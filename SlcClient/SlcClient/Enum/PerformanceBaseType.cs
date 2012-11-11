using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum PerformanceBaseType
    {
        [EnumMember(Value = "Advanced")]
        Advanced,
        [EnumMember(Value = "Proficient")]
        Proficient,
        [EnumMember(Value = "Basic")]
        Basic,
        [EnumMember(Value = "Below Basic")]
        BelowBasic,
        [EnumMember(Value = "Well Below Basic")]
        WellBelowBasic
    }
}