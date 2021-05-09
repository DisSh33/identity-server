using mi.identity.server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mi.identity.server.Data.DataAccess.Responses
{
    public class UserClaimResponse
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public ProfileViewModel User { get; set; }
    }
}
