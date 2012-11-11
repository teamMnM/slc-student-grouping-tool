using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum CalendarEventType
    {
        [EnumMember(Value = "Instructional day")]
        InstructionalDay,
        [EnumMember(Value = "Teacher only day")]
        TeacherOnlyDay,
        [EnumMember(Value = "Holiday")]
        Holiday,
        [EnumMember(Value = "Make-up day")]
        MakeUpDay,
        [EnumMember(Value = "Weather day")]
        WeatherDay,
        [EnumMember(Value = "Student late arrival/early dismissal")]
        StudentLateArrivalEarlyDismissal,
        [EnumMember(Value = "Emergency day")]
        EmergencyDay,
        [EnumMember(Value = "Strike")]
        Strike,
        [EnumMember(Value = "Other")]
        Other
    }
}