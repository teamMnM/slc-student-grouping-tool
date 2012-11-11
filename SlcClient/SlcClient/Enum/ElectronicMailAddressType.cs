using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum ElectronicMailAddressType
    {
        [EnumMember(Value = "Home/Personal")]
        HomePersonal,
        [EnumMember(Value = "Organization")]
        Organization,
        [EnumMember(Value = "Other")]
        Other,
        [EnumMember(Value = "Work")]
        Work
    }
}