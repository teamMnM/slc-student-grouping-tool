using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlcClient.Entities.Authentication
{
    public class Authentication
    {
        public UserAuthentication userAuthentication { get; set; }
        public Principal principal { get; set; }
    }
}
