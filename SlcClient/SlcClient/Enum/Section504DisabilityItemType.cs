using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SlcClient.Enum
{
    [DataContract]
    public enum Section504DisabilityItemType
    {
        [EnumMember(Value = "Attention Deficit Hyperactivity Disorder (ADHD)")]
        [Description("Attention Deficit Hyperactivity Disorder (ADHD)")]
        AttentionDeficitHyperactivityDisorderADHD,

        [EnumMember(Value = "Medical Condition")]
        [Description("Medical Condition")]
        MedicalCondition,

        [EnumMember(Value = "Motor Impairment")]
        [Description("Motor Impairment")]
        MotorImpairment,

        [EnumMember(Value = "Sensory Impairment")]
        [Description("Sensory Impairment")]
        SensoryImpairment,

        [EnumMember(Value = "Other")]
        [Description("Other")]
        Other
    }
}