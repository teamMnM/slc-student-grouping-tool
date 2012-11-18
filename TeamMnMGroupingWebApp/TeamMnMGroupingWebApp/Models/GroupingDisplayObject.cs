using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamMnMGroupingWebApp.Models
{
    public class GroupingDisplayObject
    {
        /// <summary>
        /// all the cohorts needed to display
        /// </summary>
        public IEnumerable<CohortDisplayObject> cohorts { get; set; }

        /// <summary>
        /// this includes all section every student is in
        /// </summary>
        public IEnumerable<SectionDisplayObject> sections { get; set; }

        /// <summary>
        /// a list of filters to contruct a filter for students
        /// </summary>
        public IEnumerable<Filter> filters { get; set; }

        /// <summary>
        /// A list of colors to be used to assign to groups. This list is specified in the /Data/Colors.txt file
        /// </summary>
        public IEnumerable<Color> colors { get; set; }

        /// <summary>
        /// a list of all students
        /// </summary>
        public IEnumerable<StudentDisplayObject> students { get; set; }

        public IEnumerable<DataElement> dataElements { get; set; }
    }
}