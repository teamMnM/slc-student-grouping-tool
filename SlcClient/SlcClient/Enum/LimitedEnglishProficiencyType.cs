using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SlcClient.Enum
{
    [DataContract]
    public enum LimitedEnglishProficiencyType
    {
        [EnumMember(Value = "NotLimited")]
        [Description("Not Limited")]
        NotLimited,
        [EnumMember(Value = "Limited")]
        [Description("Limited")]
        Limited,
        [EnumMember(Value = "Limited Monitored 1")]
        [Description("Limited Monitored 1")]
        LimitedMonitored1,
        [EnumMember(Value = "Limited Monitored 2")]
        [Description("Limited Monitored 2")]
        LimitedMonitored2
    }
}