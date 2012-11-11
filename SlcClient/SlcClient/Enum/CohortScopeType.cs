using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum CohortScopeType
    {
        [EnumMember(Value = "District")]
        District,
        [EnumMember(Value = "School")]
        School,
        [EnumMember(Value = "Classroom")] 
        Classroom,
        [EnumMember(Value = "Teacher")]
        Teacher,
        [EnumMember(Value = "Principal")]
        Principal,
        [EnumMember(Value = "Counselor")]
        Counselor,
        [EnumMember(Value = "Statewide")]
        Statewide
    }
}