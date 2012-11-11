using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SlcClient.Entities
{
    public class StudentIndicator
    {
        /// <summary>
        /// The date when the indicator was assigned or computed.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime beginDate { get; set; }

        /// <summary>
        /// The person, organization, or department that designated the program association.
        /// </summary>
        [StringLength(60)]
        public string designatedBy { get; set; }

        /// <summary>
        /// The date the indicator or metric was sunset or removed.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime endDate { get; set; }

        /// <summary>
        /// The value of the indicator or metric. e.g at risk
        /// </summary>
        [StringLength(35)]
        public string indicator { get; set; } 

        /// <summary>
        /// The name for a group of indicators. e.g at risk
        /// </summary>
        [StringLength(60)]
        public string indicatorGroup { get; set; }

        /// <summary>
        /// The name of the indicator or metric. e.g at risk
        /// </summary>
        [StringLength(60)]
        public string indicatorName { get; set; }
    }
}