using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum AssessmentIdentificationSystemType
    {
        [EnumMember(Value = "School")]
        School,
        [EnumMember(Value = "District")]
        District,
        [EnumMember(Value = "State")]
        State,
        [EnumMember(Value = "Federal")]
        Federal,
        [EnumMember(Value = "Other Federal")]
        OtherFederal,
        [EnumMember(Value = "Test Contractor")]
        TestContractor,
        [EnumMember(Value = "Other")]
        Other
    }
}