using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum RecognitionType
    {
        [EnumMember(Value = "Athletic awards")]
        AthleticAwards,
        [EnumMember(Value = "Awarding of units of value")]
        AwardingOfUnitsOfValue,
        [EnumMember(Value = "Citizenship award/recognition")]
        CitizenshipAwardRecognition,
        [EnumMember(Value = "Completion of requirement, but no units of value awarded")]
        CompletionOfRequirementButNoUnitsOfValueAwarded,
        [EnumMember(Value = "Certificate")]
        Certificate,
        [EnumMember(Value = "Honor award")]
        HonorAward,
        [EnumMember(Value = "Letter of commendation")]
        LetterOfCommendation,
        [EnumMember(Value = "Medals")]
        Medals,
        [EnumMember(Value = "Other")]
        MonogramLetter,
        [EnumMember(Value = "Other")]
        Points,
        [EnumMember(Value = "Other")]
        PromotionOrAdvancement,
        [EnumMember(Value = "Other")]
        Other
    }
}