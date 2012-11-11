using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum TelephoneNumberType
    {
        [EnumMember(Value = "Fax")]
        Fax,
        [EnumMember(Value = "Emergency 1")]
        Emergency1,
        [EnumMember(Value = "Emergency 2")]
        Emergency2,
        [EnumMember(Value = "Home")]
        Home,
        [EnumMember(Value = "Mobile")]
        Mobile,
        [EnumMember(Value = "Other")]
        Other,
        [EnumMember(Value = "Unlisted")]
        Unlisted,
        [EnumMember(Value = "Work")]
        Work
    }
}