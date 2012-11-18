using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlcClient.Entities
{
    public class CourseOffering
    {
        /// <summary>
        /// Course offering identifier
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The local code assigned by the School that identifies the course offering provided for the instruction of students.
        /// </summary>
        [StringLength(30)]
        public string localCourseCode { get; set; }

        /// <summary>
        /// The descriptive name given to a course of study offered in the school, if different from the CourseTitle.
        /// </summary>
        [StringLength(60)]
        public string localCourseTitle { get; set; }

        /// <summary>
        /// Reference to the session the course is offered.
        /// </summary>
        public string courseId { get; set; }

        /// <summary>
        /// Reference to the session the course is offered.
        /// </summary>
        public string sessionId { get; set; }
    }
}
