using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum CohortType
    {
        [EnumMember(Value = "Academic Intervention")]
        AcademicIntervention,
        [EnumMember(Value = "Attendance Intervention")]
        AttendanceIntervention,
        [EnumMember(Value = "Discipline Intervention")]
        DisciplineIntervention,
        [EnumMember(Value = "Classroom Pullout")]
        ClassroomPullout,
        [EnumMember(Value = "Extracurricular Activity")]
        ExtracurricularActivity,
        [EnumMember(Value = "Field Trip")]
        FieldTrip,
        [EnumMember(Value = "Principal Watch List")]
        PrincipalWatchList,
        [EnumMember(Value = "Counselor List")]
        CounselorList,
        [EnumMember(Value = "In-school Suspension")]
        InschoolSuspension,
        [EnumMember(Value = "Study Hall")]
        StudyHall,
        [EnumMember(Value = "Other")]
        Other
    }
}