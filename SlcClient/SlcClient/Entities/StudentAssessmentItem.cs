using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SlcClient.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    public class StudentAssessmentItem
    {
        /// <summary>
        /// The AssessmentItem to which this studentAssessmentItem is a response to.
        /// </summary>
        public AssessmentItem assessmentItem { get; set; }

        /// <summary>
        /// The analyzed result of a student's response to an assessment item.. For example: Correct Incorrect Met standard ...
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public AssessmentItemResultType assessmentItemResult { get; set; }

        /// <summary>
        /// A student's response to a stimulus on a test.
        /// </summary>
        public string assessmentResponse { get; set; }

        /// <summary>
        /// A meaningful raw score of the performance of an individual on an assessment item.
        /// </summary>
        public int rawScoreResult { get; set; }

        /// <summary>
        /// Indicator of the response. For example: Nonscorable response Ineffective response Effective response Partial response ...
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ResponseIndicatorType responseIndicator { get; set; }
    }
}