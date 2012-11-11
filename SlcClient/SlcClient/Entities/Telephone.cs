using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SlcClient.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SlcClient.Entities
{
    public class Telephone
    {
        /// <summary>
        /// An indication that the telephone number should be used as the principal number for an individual or organization.
        /// </summary>
        public bool primaryTelephoneNumberIndicator { get; set; }

        /// <summary>
        /// The telephone number including the area code, and extension, if applicable.
        /// </summary>
        [StringLength(24)]
        public string telephoneNumber { get; set; }

        /// <summary>
        /// The type of communication number listed for an individual or organization.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public TelephoneNumberType telephoneNumberType { get; set; }
    }
}