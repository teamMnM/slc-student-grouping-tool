using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SlcClient.Entities
{
    public class LearningStyles
    {
        /// <summary>
        /// The student's relative preference expressed as a percent to auditory learning.
        /// </summary>
        public double auditoryLearning { get; set; }

        /// <summary>
        /// The student's relative preference expressed as a percent to kinesthetic or tactile learning.
        /// </summary>
        public double tactileLearning { get; set; }

        /// <summary>
        /// The student's relative preference expressed as a percent to visual learning.
        /// </summary>
        public double visualLearning { get; set; }
    }
}