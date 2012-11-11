using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum ContentStandardType
    {
        [EnumMember(Value = "National Standard")]
        NationalStandard,
        [EnumMember(Value = "State Standard")]
        StateStandard,
        [EnumMember(Value = "College Entrance Standard")]
        CollegeEntranceStandard,
        [EnumMember(Value = "LEA Standard")]
        LEAStandard,
        [EnumMember(Value = "Texas Essential Knowledge and Skills")]
        TexasEssentialKnowledgeAndSkills,
        [EnumMember(Value = "SAT")]
        SAT,
        [EnumMember(Value = "PSAT")]
        PSAT,
        [EnumMember(Value = "ACT")]
        ACT,
        [EnumMember(Value = "Advanced Placement")]
        AdvancedPlacement,
        [EnumMember(Value = "International Baccalaureate")]
        InternationalBaccalaureate
    }
}