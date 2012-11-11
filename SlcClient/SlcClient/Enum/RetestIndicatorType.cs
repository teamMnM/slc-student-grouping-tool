using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum RetestIndicatorType
    {
        [EnumMember(Value = "Primary Administration")]
        PrimaryAdministration,
        [EnumMember(Value = "1st Retest")]
        FirstRetest,
        [EnumMember(Value = "2nd Retest")]
        SecondRetest
    }
}