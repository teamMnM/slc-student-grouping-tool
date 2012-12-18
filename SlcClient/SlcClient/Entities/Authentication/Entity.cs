using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlcClient.Entities.Authentication
{
    public class Entity
    {
        public string type { get; set; }
        public string entityId { get; set; }
        public string stagedEntityId { get; set; }
    }
}
