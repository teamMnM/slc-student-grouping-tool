using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum TermType
    {
        [EnumMember(Value = "Fall Semester")]
        FallSemester,
        [EnumMember(Value = "Spring Semester")]
        SpringSemester,
        [EnumMember(Value = "Summer Semester")]
        SummerSemester,
        [EnumMember(Value = "First Trimester")]
        FirstTrimester,
        [EnumMember(Value = "Second Trimester")]
        SecondTrimester,
        [EnumMember(Value = "Third Trimester")]
        ThirdTrimester,
        [EnumMember(Value = "Year Round")]
        YearRound,
        [EnumMember(Value = "MiniTerm")]
        MiniTerm
    }
}