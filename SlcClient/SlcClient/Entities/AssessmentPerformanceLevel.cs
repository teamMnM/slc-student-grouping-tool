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
    /// Definition of the performance levels and the associated cut scores. Three styles are supported: 
    /// 1. Specification of performance level by min and max score 
    /// 2. Specification of performance level by cut score - min only 
    /// 3. Specification of performance level without any mapping to scores
    /// </summary>
    public class AssessmentPerformanceLevel
    {
        /// <summary>
        /// The method that the administrator of the assessment uses to report the performance and achievement of all students. 
        /// It may be a qualitative method such as performance level descriptors or a quantitative method such as a numerical grade or cut score. 
        /// More than one type of reporting method may be used.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public AssessmentReportingMethodType assessmentReportingMethod { get; set; }

        /// <summary>
        /// The maximum score to make the indicated level of performance.
        /// </summary>
        public int maximumScore { get; set; }

        /// <summary>
        /// The minimum score required to make the indicated level of performance.
        /// </summary>
        public int minimumScore { get; set; }

        /// <summary>
        /// The performance level(s) defined for the assessment.
        /// </summary>
        public List<PerformanceLevelDescriptor> performanceLevelDescriptor { get; set; }
    }
}