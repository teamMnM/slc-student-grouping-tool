using SlcClient.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SlcClient.Entities
{
    public class ElectronicMail
    {
        [DataType(DataType.EmailAddress)]
        public string emailAddress { get; set; }
        public ElectronicMailAddressType emailAddressType { get; set; }
    }
}