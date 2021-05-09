using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mi.identity.server.ViewModels
{
    public class ClaimViewModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string ApiResourceName { get; set; }
        public string IdentityResourceName { get; set; }
    }
}
