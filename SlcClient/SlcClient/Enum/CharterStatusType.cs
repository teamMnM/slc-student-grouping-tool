using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum CharterStatusType
    {
        [EnumMember(Value = "School Charter")]
        SchoolCharter,
        [EnumMember(Value = "College/University Charter")]
        CollegeUniversityCharter,
        [EnumMember(Value = "Open Enrollment")]
        OpenEnrollment,
        [EnumMember(Value = "Not a Charter School")]
        NotACharterSchool
    }
}