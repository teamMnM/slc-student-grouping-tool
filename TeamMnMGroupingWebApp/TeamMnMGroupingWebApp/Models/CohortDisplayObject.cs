using SlcClient.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamMnMGroupingWebApp.Models
{
    public class CohortDisplayObject
    {
        /// <summary>
        /// the cohort in context
        /// </summary>
        public Cohort cohort { get; set; }

        /// <summary>
        /// list of students associated with the cohort
        /// </summary>
        public IEnumerable<string> students { get; set; }
        
        /// <summary>
        /// the data in the cohort custom entity
        /// </summary>
        public CohortCustom custom { get; set; } 
    }
}