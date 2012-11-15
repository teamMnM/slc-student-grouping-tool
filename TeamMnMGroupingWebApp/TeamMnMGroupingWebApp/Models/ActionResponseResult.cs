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
        /// data associated with the response
        /// </summary>
        public string data { get; set; }
    }
}