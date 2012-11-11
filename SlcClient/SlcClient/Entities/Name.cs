using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SlcClient.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SlcClient.Entities
{
    public class Name
    {
        /// <summary>
        /// A name given to an individual at birth, baptism, or during another naming ceremony, or through legal change.
        /// </summary>
        [StringLength(75)]
        public string firstName { get; set; }

        /// <summary>
        /// An appendage, if any, used to denote an individual's generation in his family (e.g., Jr., Sr., III).
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public GenerationCodeSuffixType generationCodeSuffix { get; set; }

        /// <summary>
        /// The name borne in common by members of a family.
        /// </summary>
        [StringLength(75)]
        public string lastSurName { get; set; }

        /// <summary>
        /// The person's maiden name.
        /// </summary>
        [StringLength(75)]
        public string maidenName { get; set; }

        /// <summary>
        /// A secondary name given to an individual at birth, baptism, or during another naming ceremony.
        /// </summary>
        [StringLength(75)]
        public string middleName { get; set; }

        /// <summary>
        /// A prefix used to denote the title, degree, position or seniority of the person.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PersonalTitlePrefixType personalTitlePrefix { get; set; }

        /// <summary>
        /// The evidence presented to verify one's personal identity; for example: drivers license, passport, birth certificate, etc.
        /// </summary>
        public string verification { get; set; }
    }
}