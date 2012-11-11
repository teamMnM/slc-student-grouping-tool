using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SlcClient.Enum;

namespace SlcClient.Entities
{
    public class Credits
    {
        /// <summary>
        /// The value of credits or units of value awarded for the completion of a course
        /// </summary>
        public double credit { get; set; }

        /// <summary>
        /// Conversion factor that when multiplied by the number of credits is equivalent to Carnegie units.
        /// </summary>
        public double creditConversion { get; set; }

        /// <summary>
        /// Carnegie unit, Semester hour credit, Trimester hour credit, Quarter hour credit, Nine month year hour credit, Twelve month year hour credit, Other
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CreditType creditType { get; set; }

    }
}