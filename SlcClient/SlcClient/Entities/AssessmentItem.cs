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
    /// This entity represents one of many single measures that make up an assessment.
    /// </summary>
    public class AssessmentItem
    {
        /// <summary>
        /// The correct response for the assessment item.
        /// </summary>
        [StringLength(20)]
        public string correctResponse { get; set; }

        /// <summary>
        /// The correct response for the assessment item.
        /// </summary>
        [StringLength(60)]
        public string identificationCode { get; set; }

        /// <summary>
        /// Category or type of the assessment item. For example: Multiple choice Analytic Prose ....
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ItemCategoryType itemCategory { get; set; }

        /// <summary>
        /// Learning Standard tested by this item.
        /// </summary>
        public List<LearningStandardId> learningStandardId { get; set; }

        /// <summary>
        /// The maximum raw score achievable is all assessment items are correct and scored at the maximum.
        /// </summary>
        public int maxRawScore { get; set; }

        /// <summary>
        /// The correct response for the assessment item.
        /// </summary>
        [StringLength(35)]
        public string nomenclature { get; set; }
    }
}