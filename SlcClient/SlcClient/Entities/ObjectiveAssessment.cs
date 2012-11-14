using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    /// <summary>
    /// This entity represents subtests that assess specific learning objectives.
    /// </summary>
    public class ObjectiveAssessment
    {
        /// <summary>
        /// References individual test items, if appropriate.
        /// </summary>
        public List<AssessmentItem> assessmentItem { get; set; }

        /// <summary>
        /// Definition of the performance levels and the associated cut scores. Three styles are supported: 
        /// 1. Specification of performance level by min and max score 
        /// 2. Specification of performance level by cut score - min only 
        /// 3. Specification of performance level without any mapping to scores
        /// </summary>
        public List<AssessmentPerformanceLevel> assessmentPerformanceLevel { get; set; }

        /// <summary>
        /// A unique number or alphanumeric code assigned to a space, room, site, building, individual, organization, program, 
        /// or institution by a school, school system, a state, or other agency or entity.
        /// </summary>
        [StringLength(60)]
        public string identificationCode { get; set; }

        /// <summary>
        /// The maximum raw score achievable across all assessment items that are correct and scored at the maximum.
        /// </summary>
        public int maxRawScore { get; set; }

        /// <summary>
        /// Reflects the specific nomenclature used for this level of ObjectiveAssessment.
        /// </summary>
        public string nomenclature { get; set; }

        /// <summary>
        /// Subsets of the assessment that address specific learning objectives.
        /// </summary>
        public List<ObjectiveAssessment> objectiveAssessments { get; set; }

        /// <summary>
        /// The percent of the assessment that tests this objective.
        /// </summary>
        public double percentOfAssessment { get; set; }

        /// <summary>
        /// References to a learning objective
        /// </summary>
        public List<string> learningObjectives { get; set; }

    }
}