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
    public class Disability
    {
        /// <summary>
        /// A disability category that describes a child's impairment.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public DisabilityType disability { get; set; }

        /// <summary>
        /// A description of the disability diagnosis.
        /// </summary>
        [Range(1, 80)]
        [StringLength(80)]
        public string disabilityDiagnosis { get; set; }

        /// <summary>
        /// The order by severity of student's disabilities: 1- Primary, 2 - Secondary, 3 - Tertiary, etc.
        /// </summary>
        public int orderOfDisability { get; set; }
    }
}