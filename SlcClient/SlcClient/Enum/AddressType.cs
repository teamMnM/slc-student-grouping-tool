using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum AddressType
    {
        [EnumMember(Value = "Home")]
        Home,
        [EnumMember(Value = "Physical")]
        Physical,
        [EnumMember(Value = "Billing")]
        Billing,
        [EnumMember(Value = "Mailing")]
        Mailing,
        [EnumMember(Value = "Other")]
        Other,
        [EnumMember(Value = "Temporary")]
        Temporary,
        [EnumMember(Value = "work")]
        Work
    }
}