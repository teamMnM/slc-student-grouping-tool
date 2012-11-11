using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlcClient.Entities
{
    /// <summary>
    /// The academic rank information of a student in relation to his or her graduating class
    /// </summary>
    public class ClassRanking
    {
        /// <summary>
        /// The academic rank of a student in relation to his or her graduating class (e.g., 1st, 2nd, 3rd).
        /// </summary>
        public int classRank { get; set; }

        /// <summary>
        /// Date class ranking was determined.
        /// </summary>
        public DateTime classRankingDate { get; set; }

        /// <summary>
        /// The academic percentage rank of a student in relation to his or her graduating class (e.g., 95%, 80%, 50%).
        /// </summary>
        public int percentageRanking { get; set; }

        /// <summary>
        /// The total number of students in the student's graduating class.
        /// </summary>
        public int totalNumberInClass { get; set; }
    }
}