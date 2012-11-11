using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    /// <summary>
    /// This entity holds the score and or performance levels earned for an objective assessment by a student.
    /// </summary>
    public class StudentObjectiveAssessment
    {
        /// <summary>
        /// This entity represents subtests that assess specific learning objectives.
        /// </summary>
        public ObjectiveAssessment objectiveAssessment { get; set; }

        /// <summary>
        /// This descriptor defines various levels or thresholds for performance on the assessment.
        /// </summary>
        public List<PerformanceLevelDescriptor> performanceLevelDescriptors {get; set;}

        /// <summary>
        /// A meaningful score or statistical expression of the performance of an individual. The results can be expressed as a number, percentile, range, level, etc.
        /// </summary>
        public List<ScoreResult> scoreResults { get; set; }
    }
}