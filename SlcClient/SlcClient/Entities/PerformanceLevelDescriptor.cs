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
    /// <summary>
    /// This descriptor defines various levels or thresholds for performance on the assessment.
    /// </summary>
    public class PerformanceLevelDescriptor
    {
        /// <summary>
        /// A code or abbreviation that is used to refer to a Performance Level,
        /// </summary>
        [StringLength(20)]
        public string codeValue { get; set; }

        /// <summary>
        /// A code or abbreviation that is used to refer to a Performance Level,
        /// </summary>
        [StringLength(1024)]
        public string description { get; set; }

        /// <summary>
        /// A conversion of the level to a standard set of performance levels.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PerformanceBaseType performanceBaseConversion { get; set; }
    }
}