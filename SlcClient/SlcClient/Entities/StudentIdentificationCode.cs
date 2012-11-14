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
    public class StudentIdentificationCode
    {
        /// <summary>
        /// The organization code or name assigning the student Identification Code.
        /// </summary>
        [StringLength(60)]
        public string assigningOrganizationCode { get; set; }

        /// <summary>
        /// A unique number or alphanumeric code assigned to a student by a school, school system, a state, or other agency or entity.
        /// </summary>
        [StringLength(60)]
        public string identificationCode { get; set; }

        /// <summary>
        /// A coding scheme that is used for identification and record-keeping purposes by schools, social services, or other agencies to refer to a student.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public StudentIdentificationSystemType identificationSystem { get; set; }

    }
}