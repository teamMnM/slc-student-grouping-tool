using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    /// <summary>
    /// An indication of the pre-determined criteria for granting the teaching credential that an individual holds. 
    /// </summary>
    [DataContract]
    public enum TeachingCredentialBasisType
    {
        [EnumMember(Value = "4-year bachelor's degree")]
        FourYearBachelorsDegree,
        [EnumMember(Value = "5-year bachelor's degree")]
        FiveYearBachelorsDegree,
        [EnumMember(Value = "Master's degree")]
        MastersDegree,
        [EnumMember(Value = "Doctoral degree")]
        DoctoralDegree,
        [EnumMember(Value = "Met state testing requirement")]
        MetStateTestingRequirement,
        [EnumMember(Value = "Special/alternative program completion")]
        SpecialAlternativeProgramCompletion,
        [EnumMember(Value = "Relevant experience")]
        RelevantExperience,
        [EnumMember(Value = "Credentials based on reciprocation with another state")]
        CredentialsBasedOnReciprocationWithAnotherState
    }
}