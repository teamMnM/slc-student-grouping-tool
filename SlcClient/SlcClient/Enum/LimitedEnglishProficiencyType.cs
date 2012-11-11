using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum LimitedEnglishProficiencyType
    {
        [EnumMember(Value = "NotLimited")]
        NotLimited,
        [EnumMember(Value = "Limited")]
        Limited,
        [EnumMember(Value = "Limited Monitored 1")]
        LimitedMonitored1,
        [EnumMember(Value = "Limited Monitored 2")]
        LimitedMonitored2
    }
}