using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SlcClient.Entities
{
    public class AccountabilityRating
    {
        [StringLength(35)]
        public string rating { get; set; }

        [DataType(DataType.Date)]
        public DateTime ratingDate { get; set; }

        [StringLength(35)]
        public string ratingOrganization { get; set; }

        [StringLength(30)]
        public string ratingProgram { get; set; }

        [StringLength(30)]
        public string ratingTitle { get; set; }
    }
}