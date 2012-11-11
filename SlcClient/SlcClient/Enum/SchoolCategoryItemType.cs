using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum SchoolCategoryItemType
    {
        [EnumMember(Value = "Elementary Secondary School")]
        ElementarySecondarySchool,
        [EnumMember(Value = "Elementary School")]
        ElementarySchool,
        [EnumMember(Value = "High School")]
        HighSchool,
        [EnumMember(Value = "Middle School")]
        MiddleSchool,
        [EnumMember(Value = "Junior High School")]
        JuniorHighSchool,
        [EnumMember(Value = "SecondarySchool")]
        SecondarySchool,
        [EnumMember(Value = "Ungraded")]
        Ungraded,
        [EnumMember(Value = "Adult School")]
        AdultSchool,
        [EnumMember(Value = "Infant/toddler School")]
        InfantToddlerSchool,
        [EnumMember(Value = "Preschool/early childhood")]
        PreschoolEarlyChildhood,
        [EnumMember(Value = "Primary School")]
        PrimarySchool,
        [EnumMember(Value = "Intermediate School")]
        IntermediateSchool
    }
}