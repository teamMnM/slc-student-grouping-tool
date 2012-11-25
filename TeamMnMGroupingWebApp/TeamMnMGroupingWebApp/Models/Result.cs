using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamMnMGroupingWebApp.Models
{
    public class Result
    {
        /// <summary>
        /// Main object Id
        /// </summary>
        public string objectId { get; set; }

        /// <summary>
        /// Indicates whether the entire action was completed succesfully
        /// </summary>
        public bool completedSuccessfully { get; set; }

        /// <summary>
        /// The main object action result
        /// </summary>
        public ActionResponseResult objectActionResult { get; set; }

        /// <summary>
        /// Indicates whether any association was created
        /// </summary>
        public bool partialCreateSuccess { get; set; }

        /// <summary>
        /// Indicates whether any assocation was deleted
        /// </summary>
        public bool partialDeleteSuccess { get; set; }

        /// <summary>
        /// this list contains all the Id's that the action failed to create new objects for
        /// </summary>
        public IEnumerable<ActionResponseResult> failToCreateAssocations { get; set; }

        /// <summary>
        /// this list contains all the Id's that the action failed to delete for
        /// </summary>
        public IEnumerable<ActionResponseResult> failToDeleteAssociations { get; set; }

        public ActionResponseResult customActionResult { get; set; }
    }
}