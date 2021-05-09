using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using MediatR;
using mi.identity.server.Data;
using mi.identity.server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace mi.identity.server
{
    public class Startup
    {
        public IConfiguration _configuration { get; }
        public IWebHostEnvironment _environment { get; }
        public AppSettings AppSettings { get; set; } = new AppSettings();

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 8;
                config.Password.RequireDigit = true;
                config.Password.RequireUppercase = true;
                config.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/auth/login";
                config.LogoutPath = "/auth/logout";

                config.Cookie.Path = "/";
                config.Cookie.SameSite = SameSiteMode.Unspecified;
            });
            services.ConfigureNonBreakingSameSiteCookies();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(options =>
                {                    
                    options.ConfigureDbContext = b => b.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));

                    options.IdentityResourceClaim = new TableConfiguration("IdentityResourceClaim");
                    options.ApiResourceClaim = new TableConfiguration("ApiResourceClaim");
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
#if DEBUG
                .AddDeveloperSigningCredential();
#else
                .AddSigningCredential(new X509Certificate2(Path.Combine(@"/tmp", "IdentityServerCert.pfx")));
#endif       

            services.AddAuthentication(config =>
            {
                config.DefaultScheme = "Cookie";
                config.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookie", config =>
            {
                config.Cookie.SecurePolicy = CookieSecurePolicy.None;
            })
            .AddOpenIdConnect("oidc", config => 
            {
                config.Authority = AppSettings.Common.IdentityServer.Authority;

                config.SignInScheme = "Cookie";
                config.RequireHttpsMetadata = false;

                config.ClientId = "mi.identity.server.api";
                config.ClientSecret = "secret_mi.identity.server.api";

                config.ResponseType = "code";
                config.SaveTokens = true;

                config.SignedOutCallbackPath = "/identity/signout-callback-oidc";

                config.GetClaimsFromUserInfoEndpoint = true;                               

                config.ClaimActions.MapAll();

                config.Scope.Add("shared.scope");
                config.Scope.Add("my.admin.scope");
                config.Scope.Add("openid");
                config.Scope.Add("profile");
                config.Scope.Add("offline_access");
            });

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("https://localhost")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddMediatR(typeof(Startup));

            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddControllers();

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "my.identity.server", Version = "1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UsePathBase(AppSettings.Common.Host.Root);

            app.Use(async (context, next) =>
            {
                context.Request.Scheme = "https";
                await next.Invoke();
            });

            app.UseCors("default");

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("./v1/swagger.json", "my.identity.server"));

            app.UseRouting();

            app.UseCookiePolicy();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();
  
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
                
    }
}
