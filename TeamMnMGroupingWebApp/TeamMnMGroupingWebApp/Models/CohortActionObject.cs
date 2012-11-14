using SlcClient.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamMnMGroupingWebApp.Models
{
    /// <summary>
    /// This object is used for any Create/Update action between the ajax call and the backend
    /// </summary>
    public class CohortActionObject : CohortDisplayObject
    {
        /// <summary>
        /// the list of students to delete from the cohort
        /// </summary>
        public IEnumerable<string> studentsToDelete { get; set; }

        /// <summary>
        /// the list of students to add to the cohort
        /// </summary>
        public IEnumerable<string> studentsToCreate { get; set; }
    }
}