using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum InstitutionTelephoneNumberType
    {
        [EnumMember(Value = "Main")]
        Main,
        [EnumMember(Value = "Administrative")]
        Administrative,
        [EnumMember(Value = "HealthClinic")]
        HealthClinic,
        [EnumMember(Value = "Attendance")]
        Attendance,
        [EnumMember(Value = "Other")]
        Other,
        [EnumMember(Value = "Fax")]
        Fax
    }
}