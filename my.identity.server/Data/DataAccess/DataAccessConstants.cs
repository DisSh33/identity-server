using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mi.identity.server.Data.DataAccess
{
    public class DataAccessConstants
    {
        public string DefaultApiResourceName { get; set; }
        public string DefaultIdentityResourceName { get; set; }

        public Dictionary<string, List<string>> RolePermissions { get; set; }
    }
}
