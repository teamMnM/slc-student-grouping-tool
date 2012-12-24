using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlcClient.Entities.Authentication
{
    public class Link
    {
        public string rel { get; set; }

        public string href { get; set; }
    }

    public class Home
    {
        public IEnumerable<Link> links { get; set; }
    }
}
