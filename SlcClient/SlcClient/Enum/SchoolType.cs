using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum SchoolType
    {
        [EnumMember(Value = "Alternative")]
        Alternative,
        [EnumMember(Value = "Regular")]
        Regular,
        [EnumMember(Value = "Special Education")]
        SpecialEducation,
        [EnumMember(Value = "Vocational")]
        Vocational,
        [EnumMember(Value = "JJAEP")]
        JJAEP,
        [EnumMember(Value = "DAEP")]
        DAEP
    }
}