using SlcClient.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    /// <summary>
    /// This entity represents identified learning objectives for courses in specific grades.
    /// </summary>
    public class LearningObjective
    {
        /// <summary>
        /// The description of the content or subject area (e.g., arts, mathematics, reading, stenography, or a foreign language) of an assessment.
        /// </summary>
        public AcademicSubjectType academicSubject { get; set; }

        /// <summary>
        /// The description of the learning objective.
        /// </summary>
        [StringLength(1024)]
        public string description { get; set; }

        /// <summary>
        /// The Identifier for the specific learning objective in the context of a standard (e.g., 111.15.3.1.A)
        /// </summary>
        public LearningStandardId learningObjectiveId { get; set; }

        /// <summary>
        /// The designated title of the learning objective.
        /// </summary>
        [StringLength(60)]
        public string objective { get; set; }

        /// <summary>
        /// The grade level for which the learning objective is targeted,
        /// </summary>
        public GradeLevelType objectiveGradeLevel { get; set; }

        /// <summary>
        /// Learning standard(s) included in this objective.
        /// </summary>
        public List<LearningStandard> learningStandards { get; set; }

        /// <summary>
        /// Parent learning objective this is a sub-objective for.
        /// </summary>
        public LearningObjective parentLearningObjective { get; set; }
    }
}