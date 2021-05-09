using mi.identity.server.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mi.identity.server.Data.DataAccess.Responses
{
    public class UserRoleResponse
    {
        public IdentityRole Role { get; set; }
        public ProfileViewModel User { get; set; }
    }
}
