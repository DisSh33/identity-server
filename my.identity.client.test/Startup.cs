using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace mi.identity.client.test
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config => 
            {
                config.DefaultScheme = "Cookie";
                config.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookie", config =>
            {
                config.Cookie.SecurePolicy = CookieSecurePolicy.None;
                //config.Cookie.SameSite = SameSiteMode.Unspecified;
            })
            .AddOpenIdConnect("oidc", config => {
                config.Authority = $"{AppSettings.Common.IdentityServer.Authority}";
                    
                config.SignInScheme = "Cookie";
                config.RequireHttpsMetadata = false;
                    
                config.ClientId = "my.identity.client.mvc";
                config.ClientSecret = "secret_my.identity.client.mvc_secret";
                    
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

            services.AddHttpClient();
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                context.Request.Scheme = "https";
                await next.Invoke();
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}").RequireAuthorization(); 
            });
        }
    }
}
