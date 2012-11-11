using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamMnMGroupingWebApp.Models
{
    /// <summary>
    /// The data element to display for students
    /// </summary>
    public class DataElement
    {
        /// <summary>
        /// name of the data element
        /// </summary>
        public string attributeName { get; set; }

        /// <summary>
        /// id of the data element
        /// </summary>
        public string attributeId { get; set; }
    }
}