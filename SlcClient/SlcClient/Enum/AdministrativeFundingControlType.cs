using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum AdministrativeFundingControlType
    {
        [EnumMember(Value = "Public School")]
        PublicSchool,
        [EnumMember(Value = "Private School")]
        PrivateSchool,
        [EnumMember(Value = "Other")]
        Other
    }
}