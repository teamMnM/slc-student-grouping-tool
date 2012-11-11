using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum CreditType
    {
        [EnumMember(Value = "Carnegie unit")]
        CarnegieUnit,
        [EnumMember(Value = "Semester hour credit")]
        SemesterHourCredit,
        [EnumMember(Value = "Trimester hour credit")]
        TrimesterHourCredit,
        [EnumMember(Value = "Quarter hour credit")]
        QuarterHourCredit,
        [EnumMember(Value = "Nine month year hour credit")]
        NineMonthYearHourCredit,
        [EnumMember(Value = "Twelve month year hour credit")]
        TwelveMonthYearHourCredit,
        [EnumMember(Value = "Other")]
        Other
    }
}