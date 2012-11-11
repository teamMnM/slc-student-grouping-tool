using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum GenerationCodeSuffixType
    {
        [EnumMember(Value = "Jr")]
        Jr,
        [EnumMember(Value = "Sr")]
        Sr,
        [EnumMember(Value = "II")]
        II,
        [EnumMember(Value = "III")]
        III,
        [EnumMember(Value = "IV")]
        IV,
        [EnumMember(Value = "V")]
        V,
        [EnumMember(Value = "VI")]
        VI,
        [EnumMember(Value = "VII")]
        VII,
        [EnumMember(Value = "VIII")]
        VIII
    }
}