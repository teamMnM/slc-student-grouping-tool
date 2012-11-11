using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum AssessmentItemResultType
    {
        [EnumMember(Value = "Correct")]
        Correct,
        [EnumMember(Value = "Incorrect")]
        Incorrect,
        [EnumMember(Value = "Met standard")]
        MetStandard
    }
}