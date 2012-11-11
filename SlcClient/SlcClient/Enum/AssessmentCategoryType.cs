using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum AssessmentCategoryType
    {
        [EnumMember(Value = "Achievement test")]
        AchievementTest,
        [EnumMember(Value = "Advanced Placement")]
        AdvancedPlacement,
        [EnumMember(Value = "International Baccalaureate")]
        InternationalBaccalaureate,
        [EnumMember(Value = "Aptitude test")]
        AptitudeTest,
        [EnumMember(Value = "Attitudinal test")]
        AttitudinalTest,
        [EnumMember(Value = "Benchmark test")]
        BenchmarkTest,
        [EnumMember(Value = "Class test")]
        ClassTest,
        [EnumMember(Value = "class quiz")]
        ClassQuiz,
        [EnumMember(Value = "College entrance exam")]
        CollegeEntranceExam,
        [EnumMember(Value = "Cognitive and perceptual skills test")]
        CognitiveAndPerceptualSkillsTest,
        [EnumMember(Value = "Developmental observation")]
        DevelopmentalObservation,
        [EnumMember(Value = "English proficiency screening test")]
        EnglishProficiencyScreeningTest,
        [EnumMember(Value = "Foreign language proficiency test")]
        ForeignLanguageProficiencyTest,
        [EnumMember(Value = "Interest inventory")]
        InterestInventory,
        [EnumMember(Value = "Manual dexterity test")]
        ManualDexterityTest,
        [EnumMember(Value = "Mental ability (intelligence) test")]
        MentalAbilityIintelligenceTest,
        [EnumMember(Value = "Performance assessment")]
        PerformanceAssessment,
        [EnumMember(Value = "Personality test")]
        PersonalityTest,
        [EnumMember(Value = "Portfolio assessment")]
        PortfolioAssessment,
        [EnumMember(Value = "Psychological test")]
        PsychologicalTest,
        [EnumMember(Value = "Psychomotor test")]
        PsychomotorTest,
        [EnumMember(Value = "Reading readiness test")]
        ReadingReadinessTest,
        [EnumMember(Value = "State summative assessment 3-8 general")]
        StateSummativeAssessment38General,
        [EnumMember(Value = "State high school subject assessment")]
        StateHighSchoolSubjectAssessment,
        [EnumMember(Value = "State high school course assessment")]
        StateHighSchoolCourseAssessment,
        [EnumMember(Value = "State alternative assessment/grade-level standards")]
        StateAlternativeAssessmentGradeLevelStandards,
        [EnumMember(Value = "State alternative assessment/modified standards")]
        StateAlternativeAssessmentModifiedStandards,
        [EnumMember(Value = "State alternate assessment/ELL")]
        StateAlternateAssessmentELL,
        [EnumMember(Value = "State English proficiency test")]
        StateEnglishProficiencyTest,
        [EnumMember(Value = "Other")]
        Other
    }
}