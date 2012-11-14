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
    public class InstitutionTelephone
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public InstitutionTelephoneNumberType institutionTelephoneNumberType { get; set; }
        
        [StringLength(24)]
        public string telephoneNumber { get; set; }
    }
}