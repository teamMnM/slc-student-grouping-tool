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

        public Filter() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">attributeId</param>
        /// <param name="name">attributeName</param>
        /// <param name="ops">operators</param>
        public Filter(string id, string name, string[] ops)
        {
            attributeId = id;
            attributeName = name;
            operators = ops;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">attributeId</param>
        /// <param name="name">attributeName</param>
        /// <param name="ops">operators</param>
        /// <param name="vals">values</param>
        public Filter(string id, string name, string[] ops, IEnumerable<FilterValue> vals)
        {
            attributeId = id;
            attributeName = name;
            operators = ops;
            values = vals;
        }
    }
}