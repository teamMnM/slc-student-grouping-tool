using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    public enum MediumOfInstructionType
    {
        [EnumMember(Value = "Televised")]
        Televised,
        [EnumMember(Value = "Telepresence/video conference")]
        TelepresenceVideoConference,
        [EnumMember(Value = "Videotaped/prerecorded video")]
        VideotapedPrerecordedVideo,
        [EnumMember(Value = "Other technology-based instruction")]
        OtherTechnologyBasedInstruction,
        [EnumMember(Value = "Technology-based instruction in classroom")]
        TechnologyBasedInstructionInClassroom,
        [EnumMember(Value = "Correspondence instruction")]
        CorrespondenceInstruction,
        [EnumMember(Value = "Face-to-face instruction")]
        FaceToFaceInstruction,
        [EnumMember(Value = "Virtual/On-line Distance learning")]
        VirtualOnlineDistanceLearning,
        [EnumMember(Value = "Center-based instruction")]
        CenterBasedInstruction,
        [EnumMember(Value = "Independent study")]
        IndependentStudy,
        [EnumMember(Value = "Internship")]
        Internship,
        [EnumMember(Value = "Other")]
        Other
    }
}