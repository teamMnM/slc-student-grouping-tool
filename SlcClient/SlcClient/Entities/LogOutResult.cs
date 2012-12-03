using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlcClient.Entities
{
    public class LogOutResult
    {
        /// <summary>
        /// Indicates whether logout was successful
        /// </summary>
        public bool logout { get; set; }

        /// <summary>
        /// Logout result message
        /// </summary>
        public string msg { get; set; }
    }
}
