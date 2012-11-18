using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamMnMGroupingWebApp.Models
{
    public class Result
    {
        public string objectId { get; set; }

        public bool completedSuccessfully { get; set; }

        public ActionResponseResult objectActionResult { get; set; }

        public bool partialCreateSuccess { get; set; }

        public bool partialDeleteSuccess { get; set; }

        /// <summary>
        /// this list contains all the Id's that the action failed to create new objects for
        /// </summary>
        public IEnumerable<ActionResponseResult> failToCreateIds { get; set; }

        /// <summary>
        /// this list contains all the Id's that the action failed to delete for
        /// </summary>
        public IEnumerable<ActionResponseResult> failToDeleteIds { get; set; }
    }
}