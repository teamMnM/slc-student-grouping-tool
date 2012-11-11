using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SlcClient.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    public class AssessmentIdentificationCode
    {
        /// <summary>
        /// The organization code or name assigning the assessment identification code.
        /// </summary>
        [StringLength(60)]
        public string assigningOrganizationCode { get; set; }

        /// <summary>
        /// A unique number or alphanumeric code assigned to an assessment by a school, school system, a state, or other agency or entity.
        /// </summary>
        [StringLength(60)]
        public string ID { get; set; }

        /// <summary>
        /// A coding scheme that is used for identification and record-keeping purposes by schools, social services, or other agencies to refer to an assessment.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public AssessmentIdentificationSystemType identificationSystem { get; set; }
    }
}