using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum AdministrationEnvironmentType
    {
        [EnumMember(Value = "Classroom")]
        Classroom,
        [EnumMember(Value = "School")]
        School,
        [EnumMember(Value = "Testing Center")]
        TestingCenter
    }
}