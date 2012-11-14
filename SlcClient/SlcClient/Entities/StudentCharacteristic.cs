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
    /// <summary>
    /// Reflects important characteristics of the student's home situation: Displaced Homemaker, Homeless, Immigrant, 
    /// Migratory, Military Parent, Pregnant Teen, Single Parent, Unaccompanied Youth
    /// </summary>
    public class StudentCharacteristic
    {
        /// <summary>
        /// The date the characteristic was designated.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime beginDate { get; set; }

        /// <summary>
        /// The characteristic designated for the student.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public StudentCharacteristicType characteristic { get; set; }

        /// <summary>
        /// The person, organization, or department that designated the characteristic.
        /// </summary>
        [StringLength(60)]
        public string designatedBy { get; set; }

        /// <summary>
        /// The date the characteristic was removed.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime endDate { get; set; }
    }
}