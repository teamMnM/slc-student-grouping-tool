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
    /// A meaningful raw score or statistical expression of the performance of an individual. 
    /// The results can be expressed as a number, percentile, range, level, etc.
    /// </summary>
    public class ScoreResult
    {
        /// <summary>
        /// The method that the administrator of the assessment uses to report the performance and achievement of all students. 
        /// It may be a qualitative method such as performance level descriptors or a quantitative method such as a numerical grade or cut score. 
        /// More than one type of reporting method may be used.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public AssessmentReportingMethodType assessmentReportingMethod { get; set; }

        /// <summary>
        /// The value of a meaningful raw score or statistical expression of the performance of an individual. 
        /// The results can be expressed as a number, percentile, range, level, etc.
        /// </summary>
        [StringLength(35)]
        public string result { get; set; }
    }
}