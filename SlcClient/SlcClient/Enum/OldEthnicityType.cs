using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SlcClient.Enum
{
    [DataContract]
    public enum OldEthnicityType
    {
        [EnumMember(Value = "American Indian Or Alaskan Native")]
        [Description("American Indian Or Alaskan Native")]
        AmericanIndianOrAlaskanNative,
        [EnumMember(Value = "Asian Or Pacific Islander")]
        [Description("Asian Or Pacific Islander")]
        AsianOrPacificIslander,
        [EnumMember(Value = "Black, Not Of Hispanic Origin")]
        [Description("Black, Not Of Hispanic Origin")]
        BlackNotOfHispanicOrigin,
        [EnumMember(Value = "Hispanic")]
        [Description("Hispanic")]
        Hispanic,
        [EnumMember(Value = "White, Not Of Hispanic Origin")]
        [Description("White, Not Of Hispanic Origin")]
        WhiteNotOfHispanicOrigin
    }
}