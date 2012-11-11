using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SlcClient.Enum
{
    public enum AcademicHonorsType
    {
        [EnumMember(Value = "Honor roll")]
        [Description("Honor roll")]
        Honorroll,
        [EnumMember(Value = "Honor society")]
        [Description("Honor society")]
        HonorSociety,
        [EnumMember(Value = "Honorable mention")]
        [Description("Honorable mention")]
        HonorableMention,
        [EnumMember(Value = "Honors program")]
        [Description("Honors program")]
        HonorsProgram,
        [EnumMember(Value = "Prize awards")]
        [Description("Prize awards")]
        PrizeAwards,
        [EnumMember(Value = "Scholarships")]
        Scholarships,
        [EnumMember(Value = "Awarding of units of value")]
        AwardingOfUnitsOfValue,
        [EnumMember(Value = "Citizenship award/recognition")]
        CitizenshipAwardRecognition,
        [EnumMember(Value = "Completion of requirement, but no units of value awarded")]
        CompletionOfRequirementButNoUnitsOfValueAwarded,
        [EnumMember(Value = "Attendance award")]
        AttendanceAward,
        [EnumMember(Value = "Certificate")]
        Certificate,
        [EnumMember(Value = "Honor award")]
        HonorAward,
        [EnumMember(Value = "Letter of student commendation")]
        LetterOfStudentCommendation,
        [EnumMember(Value = "Medals")]
        Medals,
        [EnumMember(Value = "National Merit scholar")]
        NationalMeritScholar,
        [EnumMember(Value = "Points")]
        Points,
        [EnumMember(Value = "Promotion or advancement")]
        PromotionOrAdvancement,
        [EnumMember(Value = "Other")]
        Other
    }
}