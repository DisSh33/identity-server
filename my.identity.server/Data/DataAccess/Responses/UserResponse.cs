using mi.identity.server.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mi.identity.server.Data.DataAccess.Responses
{
    public class UserResponse
    {
        public ProfileViewModel User { get; set; }
        public IdentityRole UserRole { get; set; }
    }
}
