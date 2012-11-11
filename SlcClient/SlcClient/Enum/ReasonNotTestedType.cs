using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum ReasonNotTestedType
    {
        [EnumMember(Value = "Absent")]
        Absent,
        [EnumMember(Value = "LEP exempt")]
        LEPExempt,
        [EnumMember(Value = "LEP postponement")]
        LEPPostponement,
        [EnumMember(Value = "Not appropriate (ARD decision)")]
        NotAppropriateARDDecision,
        [EnumMember(Value = "Not tested (ARD decision)")]
        NotTestedARDDecision,
        [EnumMember(Value = "Alternate assessment administered")]
        AlternateAssessmentAdministered,
        [EnumMember(Value = "Parental waiver")]
        ParentalWaiver,
        [EnumMember(Value = "Foreign exchange student waiver")]
        ForeignExchangeStudentWaiver,
        [EnumMember(Value = "Refusal by parent")]
        RefusalByParent,
        [EnumMember(Value = "Refusal by student")]
        RefusalByStudent,
        [EnumMember(Value = "Medical waiver")]
        MedicalWaiver,
        [EnumMember(Value = "Disruptive behavior")]
        DisruptiveBehavior,
        [EnumMember(Value = "Previously passed the examination")]
        PreviouslyPassedTheExamination,
        [EnumMember(Value = "Other")]
        Other
    }
}