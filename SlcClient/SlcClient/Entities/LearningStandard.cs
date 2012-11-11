using SlcClient.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    /// <summary>
    /// This entity is a subelement of a learning objective consisting of a precise statement of the expectation of a student's proficiency.
    /// </summary>
    public class LearningStandard
    {
        /// <summary>
        /// A reference to the type of content standard e.g., state, national, etc.)
        /// </summary>
        public ContentStandardType contentStandard { get; set; }

        /// <summary>
        /// The official Course Title with which this learning standard is associated.
        /// </summary>
        [StringLength(60)]
        public string courseTitle { get; set; }

        /// <summary>
        /// A detailed statement of the expectation that is is assessed.
        /// </summary>
        [StringLength(1024)]
        public string description { get; set; }

        /// <summary>
        /// The grade level for the specific learning standard.
        /// </summary>
        public GradeLevelType gradeLevel { get; set; }

        /// <summary>
        /// The Identifier for the specific learning standard (e.g., 111.15.3.1.A)
        /// </summary>
        public LearningStandardId learningStandardId { get; set; }

        /// <summary>
        /// Subject area for the learning standard.
        /// </summary>
        public AcademicSubjectType subjectArea { get; set; }
    }
}