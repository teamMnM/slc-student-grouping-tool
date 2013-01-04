using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamMnMGroupingWebApp.Models
{
    public class FtpFile
    {
        public string Name {get;set;}
        public byte[] Content { get; set; }
        public string Type { get; set; }
    }
}