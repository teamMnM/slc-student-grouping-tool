using SlcClient.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    /// <summary>
    /// Recognition given to the student for accomplishments in a co-curricular, or extra-curricular activity.
    /// </summary>
    public class Recognition
    {
        /// <summary>
        /// The date the recognition was awarded or earned.
        /// </summary>
        public DateTime recognitionAwardDate { get; set; }

        /// <summary>
        /// A description of the type of academic distinctions earned by or awarded to the individual.
        /// </summary>
        [StringLength(80)]
        public string recognitionDescription { get; set; }

        /// <summary>
        /// The nature of recognition given to the student for accomplishments in a co-curricular, or extra-curricular activity.
        /// </summary>
        public RecognitionType recognitionType { get; set; }
    }
}