using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SlcClient.Entities
{
    public class BirthData
    {
        [DataType(DataType.DateTime)]
        public DateTime birthDate { get; set; }

        [Range(2, 30)]
        [StringLength(30)]
        public string cityOfBirth { get; set; } //2-30

        [Range(1, 2)]
        [StringLength(2)]
        public string countryOfBirthCode { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime dateEnteredUS { get; set; }

        public bool multipleBirthStatus { get; set; }

        [StringLength(30)]
        public string stateOfBirthAbbreviation { get; set; }
    }
}