using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum ResponseIndicatorType
    {
        [EnumMember(Value = "Nonscorable response")]
        NonscorableResponse,
        [EnumMember(Value = "Ineffective response")]
        IneffectiveResponse,
        [EnumMember(Value = "Effective response")]
        EffectiveResponse,
        [EnumMember(Value = "Partial response")]
        PartialResponse
    }
}