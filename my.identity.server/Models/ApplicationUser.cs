using Microsoft.AspNetCore.Identity;

namespace mi.identity.server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
