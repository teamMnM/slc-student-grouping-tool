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
    public class AcademicHonor
    {
        /// <summary>
        /// A designation of the type of academic distinctions earned by or awarded to the student.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public AcademicHonorsType academicHonorsType { get; set; }

        /// <summary>
        /// The date the honor was awarded or earned.
        /// </summary>
        public DateTime honorAwardDate { get; set; }

        /// <summary>
        /// A description of the type of academic distinctions earned by or awarded to the individual.
        /// </summary>
        [StringLength(80)]
        public string honorsDescription { get; set; }
    }
}