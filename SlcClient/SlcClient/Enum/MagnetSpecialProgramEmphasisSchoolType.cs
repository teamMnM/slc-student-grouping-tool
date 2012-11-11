using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    /// <summary>
    /// A school that has been designed: 1) to attract students of different racial/ethnic backgrounds for the purpose of reducing, preventing, 
    /// or eliminating racial isolation; and/or 2)to provide an academic or social focus on a particular theme 
    /// (e.g., science/math, performing arts, gifted/talented, or foreign language). 
    /// </summary>
    [DataContract]
    public enum MagnetSpecialProgramEmphasisSchoolType
    {
        [EnumMember(Value = "All students participate")]
        AllStudentsParticipate,
        [EnumMember(Value = "No students participate")]
        NoStudentsParticipate,
        [EnumMember(Value = "Some, but not all, students participate")]
        SomeButNotAllStudentsParticipate
    }
}