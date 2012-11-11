using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum CredentialType
    {
        [EnumMember(Value = "Certification")]
        Certification,
        [EnumMember(Value = "Endorsement")]
        Endorsement,
        [EnumMember(Value = "Licensure")]
        Licensure,
        [EnumMember(Value = "Other")]
        Other,
        [EnumMember(Value = "Registration")]
        Registration
    }
}