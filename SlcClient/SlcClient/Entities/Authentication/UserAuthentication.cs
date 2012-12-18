using SlcClient.Entities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlcClient.Entities
{
    public class UserAuthentication
    {
        public bool authenticated { get; set; }
        public Principal principal { get; set; }
    }
}
