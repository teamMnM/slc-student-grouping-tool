using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlcClient.Entities.Authentication
{
    public class Principal
    {
        public string id { get; set; }
        public string name { get; set; }
        public string realm { get; set; }
        public string externalId { get; set; }
        public string adminRealm { get; set; }
        public string edOrg { get; set; }
        public string tenantId { get; set; }
        public string sessionId { get; set; }
        public Entity entity { get; set; }
    }
}
