using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SlcClient.Enum;

namespace SlcClient.Entities
{
    public class Address
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public AddressType addressType { get; set; }

        [Range(1, 20)]
        [StringLength(20)]
        public string apartmentRoomSuiteNumber { get; set; }//1-20

        [Range(1, 20)]
        [StringLength(20)]
        public string buildingSiteNumber { get; set; } //1-20

        [Range(2, 30)]
        [StringLength(30)]
        public string city { get; set; } //2-30

        [DataType(DataType.Date)]
        public DateTime closeDate { get; set; }

        [Range(1, 2)]
        [StringLength(2)]
        public string countryCode { get; set; }

        [Range(3, 5)]
        [StringLength(5)]
        public string countyFIPSCode { get; set; }

        [Range(1, 20)]
        [StringLength(20)]
        public string latitude { get; set; }

        [Range(1, 20)]
        [StringLength(20)]
        public string longitude { get; set; }

        [Range(1, 30)]
        [StringLength(30)]
        public string nameOfCountry { get; set; }

        [DataType(DataType.Date)]
        public DateTime openDate { get; set; }

        [Range(1, 17)]
        [StringLength(17)]
        public string postalCode { get; set; }

        [Range(1, 2)]
        [StringLength(2)]
        public string stateAbbreviation { get; set; }

        [Range(1, 150)]
        [StringLength(150)]
        public string streetNumberName { get; set; }
    }
}