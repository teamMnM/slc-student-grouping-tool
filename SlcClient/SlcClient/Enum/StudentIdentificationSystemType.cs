using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum StudentIdentificationSystemType
    {
        [EnumMember(Value = "Canadian SIN")]
        CanadianSIN,
        [EnumMember(Value = "District")]
        District,
        [EnumMember(Value = "Family")]
        Family,
        [EnumMember(Value = "Federal")]
        Federal,
        [EnumMember(Value = "Local")]
        Local,
        [EnumMember(Value = "National Migrant")]
        NationalMigrant,
        [EnumMember(Value = "Other")]
        Other,
        [EnumMember(Value = "School")]
        School,
        [EnumMember(Value = "SSN")]
        SSN,
        [EnumMember(Value = "State")]
        State,
        [EnumMember(Value = "State Migrant")]
        StateMigrant
    }
}