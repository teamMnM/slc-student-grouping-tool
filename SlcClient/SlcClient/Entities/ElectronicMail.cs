using SlcClient.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SlcClient.Entities
{
    public class ElectronicMail
    {
        [DataType(DataType.EmailAddress)]
        public string emailAddress { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ElectronicMailAddressType emailAddressType { get; set; }
    }
}