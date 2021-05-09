using mi.identity.server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mi.identity.server.Data.DataAccess.Responses
{
    public class ClaimResponse
    {
        public string Type { get; set; }
        public ResourceViewModel ApiResource { get; set; }
        public ResourceViewModel IdentityResource { get; set; }
    }
}
