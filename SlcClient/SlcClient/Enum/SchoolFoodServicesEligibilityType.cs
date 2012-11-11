using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SlcClient.Enum
{
    [DataContract]
    public enum SchoolFoodServicesEligibilityType
    {
        [EnumMember(Value = "Free")]
        [Description("Free")]
        Free,
        [EnumMember(Value = "Full price")]
        [Description("Full price")]
        FullPrice,
        [EnumMember(Value = "Reduced price")]
        [Description("Reduced price")]
        ReducedPrice,
        [EnumMember(Value = "Unknown")]
        [Description("Unknown")]
        Unknown
    }
}