using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SlcClient.Enum
{
    [DataContract]
    public enum RaceItemType
    {
        [EnumMember(Value = "American Indian - Alaskan Native")]
        [Description("American Indian - Alaskan Native")]
        AmericanIndianAlaskanNative,
        [EnumMember(Value = "Asian")]
        [Description("Asian")]
        Asian,
        [EnumMember(Value = "Black - African American")]
        [Description("Black - African American")]
        BlackAfricanAmerican,
        [EnumMember(Value = "Native Hawaiian - Pacific Islander")]
        [Description("Native Hawaiian - Pacific Islander")]
        NativeHawaiianPacificIslander,
        [EnumMember(Value = "White")]
        [Description("White")]
        White
    }
}