using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum TitleIPartASchoolDesignationType
    {
        [EnumMember(Value = "Not designated as a Title I Part A school")]
        NotDesignatedAsATitleIPartASchool,
        [EnumMember(Value = "Title I Part A Schoolwide Assistance Program School")]
        TitleIPartASchoolwideAssistanceProgramSchool,
        [EnumMember(Value = "Title I Part A Targeted Assistance School")]
        TitleIPartATargetedAssistanceSchool,
        [EnumMember(Value = "Title I targeted eligible school - no program")]
        TitleITargetedEligibleSchoolNoProgram,
        [EnumMember(Value = "Title I targeted school")]
        TitleITargetedSchool,
        [EnumMember(Value = "Title I school wide eligible - Title I targeted program")]
        TitleISchoolWideEligibleTitleITargetedProgram,
        [EnumMember(Value = "Title I school wide eligible school - no program")]
        TitleISchoolWideEligibleSchoolNoProgram
    }
}