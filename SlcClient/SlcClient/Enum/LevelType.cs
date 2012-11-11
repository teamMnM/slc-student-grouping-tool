using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum LevelType
    {
        [EnumMember(Value = "All Level (Grade Level PK-12)")]
        AllLevelGradeLevelPK12,
        [EnumMember(Value = "All-Level (Grade Level EC-12)")]
        AllLevelGradeLevelEC12,
        [EnumMember(Value = "Early Childhood (PK-K)")]
        EarlyChildhoodPKK,
        [EnumMember(Value = "Elementary (Grade Level 1-6)")]
        ElementaryGradeLevel16,
        [EnumMember(Value = "Elementary (Grade Level 1-8)")]
        ElementaryGradeLevel18,
        [EnumMember(Value = "Elementary (Grade Level 4-8)")]
        ElementaryGradeLevel48,
        [EnumMember(Value = "Elementary (Grade Level EC-4)")]
        ElementaryGradeLevelEC4,
        [EnumMember(Value = "Elementary (Grade Level EC-6)")]
        ElementaryGradeLevelEC6,
        [EnumMember(Value = "Elementary (Grade Level PK-5)")]
        ElementaryGradeLevelPK5,
        [EnumMember(Value = "Elementary (Grade Level PK-6)")]
        ElementaryGradeLevelPK6,
        [EnumMember(Value = "Grade Level NA")]
        GradeLevelNA,
        [EnumMember(Value = "Junior High (Grade Level 6-8)")]
        JuniorHighGradeLevel68,
        [EnumMember(Value = "Secondary (Grade Level 6-12)")]
        SecondaryGradeLevel612,
        [EnumMember(Value = "Secondary (Grade Level 8-12)")]
        SecondaryGradeLevel812
    }
}