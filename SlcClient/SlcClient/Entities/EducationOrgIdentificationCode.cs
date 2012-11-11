using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SlcClient.Entities
{
    public class EducationOrgIdentificationCode
    {
        [StringLength(60)]
        public string Id { get; set; }

        public string identificationSystem { get; set; }
    }
}