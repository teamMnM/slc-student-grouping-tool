using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamMnMGroupingWebApp.Models
{
    /// <summary>
    /// This is used to contruct filter objects to filter data on the front end
    /// attributeName: 'Disability',
    /// attributeId: 'disabilities',
    /// operators: ['contains'],
    /// values: ['Autistic', 'Deafness', 'Emotional Disturbance']
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// display name of the filter
        /// </summary>
        public string attributeName { get; set; }

        /// <summary>
        /// the property name on the object to filter
        /// </summary>
        public string attributeId { get; set; }

        /// <summary>
        /// the operators that can be applied to this filter. e.g =, >, <, >=, <=, contains
        /// </summary>
        public string[] operators { get; set; }

        /// <summary>
        /// list of values for this object property
        /// </summary>
        public IEnumerable<FilterValue> values { get; set; }
    }
}