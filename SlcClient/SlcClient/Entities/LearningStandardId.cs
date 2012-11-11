using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    /// <summary>
    /// The identifier for the learning standard.
    /// </summary>
    public class LearningStandardId
    {
        /// <summary>
        /// The name of the content standard from which the learning standard was derived.
        /// </summary>
        [StringLength(65)]
        public string contentStandardName { get; set; }

        /// <summary>
        /// A unique number or alphanumeric code assigned to a learning Standard.
        /// </summary>
        [StringLength(60)]
        public string identificationCode { get; set; }
    }
}