using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace TeamMnMGroupingWebApp.Models
{
    public class ActionResponseResult
    {
        /// <summary>
        /// result status of the request
        /// </summary>
        public HttpStatusCode status { get; set; }

        /// <summary>
        /// Object Id associated with the response
        /// </summary>
        public string objectId { get; set; }

        /// <summary>
        /// Object name associated with the response
        /// </summary>
        public string objectName { get; set; }

        /// <summary>
        /// Response message
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// Indicates whether this action was a success
        /// </summary>
        public bool isSuccess { get; set; }
    }
}