using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    /// <summary>
    /// An indication as to whether a student has previously taken a given course. 
    /// </summary>
    [DataContract]
    public enum RepeatIdentifierType
    {
        [EnumMember(Value = "Repeated, counted in grade point average")]
        RepeatedCountedInGradePointAverage,
        [EnumMember(Value = "Repeated, not counted in grade point average")]
        RepeatedNotCountedInGradePointAverage,
        [EnumMember(Value = "Not repeated")]
        NotRepeated,
        [EnumMember(Value = "Other")]
        Other
    }
}