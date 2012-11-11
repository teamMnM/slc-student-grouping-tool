using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    public enum EducationalEnvironmentType
    {
        [EnumMember(Value = "Classroom")]
        Classroom,
        [EnumMember(Value = "Homebound")]
        Homebound,
        [EnumMember(Value = "Hospital class")]
        HospitalClass,
        [EnumMember(Value = "In-school suspension")]
        InSchoolSuspension,
        [EnumMember(Value = "Laboratory")]
        Laboratory,
        [EnumMember(Value = "Mainstream (Special Education)")]
        MainstreamSpecialEducation,
        [EnumMember(Value = "Off-school center")]
        OffSchoolCenter,
        [EnumMember(Value = "Pull-out class")]
        PullOutClass,
        [EnumMember(Value = "Resource room")]
        ResourceRoom,
        [EnumMember(Value = "Self-contained (Special Education)")]
        SelfContainedSpecialEducation,
        [EnumMember(Value = "Self-study")]
        SelfStudy,
        [EnumMember(Value = "Other")]
        Other
    }
}