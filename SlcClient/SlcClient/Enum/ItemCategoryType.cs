using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum ItemCategoryType
    {
        [EnumMember(Value = "Analytic")]
        Analytic,
        [EnumMember(Value = "List Question")]
        ListQuestion,
        [EnumMember(Value = "Math Matrix")]
        MathMatrix,
        [EnumMember(Value = "Matching")]
        Matching,
        [EnumMember(Value = "Multiple Choice")]
        MultipleChoice,
        [EnumMember(Value = "Prose")]
        Prose,
        [EnumMember(Value = "Rubric")]
        Rubric,
        [EnumMember(Value = "True-False")]
        TrueFalse
    }
}