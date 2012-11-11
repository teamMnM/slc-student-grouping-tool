using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SlcClient.Entities
{
    public class ServiceDescriptorType
    {
        [StringLength(20)]
        public string codeValue { get; set; }

        [StringLength(1024)]
        public string description { get; set; }

        [StringLength(50)]
        public string shortDescription { get; set; }
    }
}