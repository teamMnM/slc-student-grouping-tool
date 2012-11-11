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
    public class ProgramParticipation
    {
        [DataType(DataType.Date)]
        public DateTime beginDate { get; set; }

        [StringLength(60)]
        public string designatedBy { get; set; }

        [DataType(DataType.Date)]
        public DateTime endDate { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ProgramType programType { get; set; }

    }
}