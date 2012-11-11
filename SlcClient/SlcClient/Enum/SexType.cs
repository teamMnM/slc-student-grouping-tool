using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SlcClient.Enum
{
    [DataContract]
    public enum SexType
    {
        [EnumMember(Value = "Female")]
        [Description("Female")]
        Female,

        [EnumMember(Value = "Male")]
        [Description("Male")]
        Male
    }
}