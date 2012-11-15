using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlcClient.Entities
{
    public class UserSession
    {
        public bool authenticated { get; set; }

        public EducationOrganization edOrg { get; set; }

        public string edOrgId { get; set; }

        public string email { get; set; }

        public string external_id { get; set; }

        public string full_name { get; set; }

        public List<string> granted_authorities { get; set; }

        public string realm { get; set; }

        public List<string> rights { get; set; }

        public List<string> sliRoles { get; set; }

        public string tenantId { get; set; }

        public string user_id { get; set; }

        public string redirect_user { get; set; } //when authenticated is false

        public List<string> all_roles { get; set; }
    }
}
