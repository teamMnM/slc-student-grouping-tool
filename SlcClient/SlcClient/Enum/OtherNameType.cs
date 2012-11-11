using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum OtherNameType
    {
        [EnumMember(Value = "Alias")]
        Alias,
        [EnumMember(Value = "Nickname")]
        Nickname,
        [EnumMember(Value = "Other Name")]
        OtherName,
        [EnumMember(Value = "Previous Legal Name")]
        PreviousLegalName
    }
}