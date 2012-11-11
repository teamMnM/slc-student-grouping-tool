using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum LevelOfEducationType
    {
        [EnumMember(Value = "No Degree")]
        NoDegree,
        [EnumMember(Value = "Bachelor's")]
        Bachelors,
        [EnumMember(Value = "Master's")]
        Masters,
        [EnumMember(Value = "Doctorate")]
        Doctorate
    }
}