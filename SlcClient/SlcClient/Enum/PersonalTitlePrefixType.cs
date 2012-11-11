using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum PersonalTitlePrefixType
    {
        [EnumMember(Value = "Colonel")]
        Colonel,
        [EnumMember(Value = "Dr")]
        Dr,
        [EnumMember(Value = "Mr")]
        Mr,
        [EnumMember(Value = "Mrs")]
        Mrs,
        [EnumMember(Value = "Ms")]
        Ms,
        [EnumMember(Value = "Reverend")]
        Reverend,
        [EnumMember(Value = "Sr")]
        Sr,
        [EnumMember(Value = "Sister")]
        Sister
    }
}