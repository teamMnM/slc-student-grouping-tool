using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum AssessmentReportingMethodType
    {
        [EnumMember(Value = "Achievement/proficiency level")]
        AchievementProficiencyLevel,
        [EnumMember(Value = "ACT score")]
        ACTScore,
        [EnumMember(Value = "Adaptive scale score")]
        AdaptiveScaleScore,
        [EnumMember(Value = "Age score")]
        AgeScore,
        [EnumMember(Value = "C-scaled scores")]
        CScaledScores,
        [EnumMember(Value = "College Board examination scores")]
        CollegeBoardExaminationScores,
        [EnumMember(Value = "Composite Score")]
        CompositeScore,
        [EnumMember(Value = "Composite Rating")]
        CompositeRating,
        [EnumMember(Value = "Composition Score")]
        CompositionScore,
        [EnumMember(Value = "Grade equivalent or grade-level indicator")]
        GradeEquivalentOrGradeLevelIndicator,
        [EnumMember(Value = "Graduation score")]
        GraduationScore,
        [EnumMember(Value = "Growth/value-added/indexing")]
        GrowthValueAddedIndexing,
        [EnumMember(Value = "International Baccalaureate score")]
        InternationalBaccalaureateScore,
        [EnumMember(Value = "Letter grade/mark")]
        LetterGradeMark,
        [EnumMember(Value = "Mastery level")]
        MasteryLevel,
        [EnumMember(Value = "Normal curve equivalent")]
        NormalCurveEquivalent,
        [EnumMember(Value = "Normalized standard score")]
        NormalizedStandardScore,
        [EnumMember(Value = "Number score")]
        NumberScore,
        [EnumMember(Value = "Pass-fail")]
        PassFail,
        [EnumMember(Value = "Percentile")]
        Percentile,
        [EnumMember(Value = "Percentile rank")]
        PercentileRank,
        [EnumMember(Value = "Proficiency level")]
        ProficiencyLevel,
        [EnumMember(Value = "Promotion score")]
        PromotionScore,
        [EnumMember(Value = "Ranking")]
        Ranking,
        [EnumMember(Value = "Ratio IQ's")]
        RatioIQs,
        [EnumMember(Value = "Raw score")]
        RawScore,
        [EnumMember(Value = "Scale score")]
        ScaleScore,
        [EnumMember(Value = "Standard age score")]
        StandardAgeScore,
        [EnumMember(Value = "Standard error measurement")]
        StandardErrorMeasurement,
        [EnumMember(Value = "Stanine score")]
        StanineScore,
        [EnumMember(Value = "Sten score")]
        StenScore,
        [EnumMember(Value = "Theta")]
        Theta,
        [EnumMember(Value = "T-score")]
        TScore,
        [EnumMember(Value = "Vertical score")]
        VerticalScore,
        [EnumMember(Value = "Workplace readiness score")]
        WorkplaceReadinessScore,
        [EnumMember(Value = "Z-score")]
        ZScore,
        [EnumMember(Value = "Other")]
        Other,
        [EnumMember(Value = "Not applicable")]
        NotApplicable
    }
}