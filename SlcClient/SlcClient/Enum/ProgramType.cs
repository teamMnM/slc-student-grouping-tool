using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum ProgramType
    {
        [EnumMember(Value = "Adult/Continuing Education")]
        AdultContinuingEducation,
        [EnumMember(Value = "Alternative Education")]
        AlternativeEducation,
        [EnumMember(Value = "Athletics")]
        Athletics,
        [EnumMember(Value = "Bilingual")]
        Bilingual,
        [EnumMember(Value = "Bilingual Summer")]
        BilingualSummer,
        [EnumMember(Value = "Career and Technical Education")]
        CareerAndTechnicalEducation,
        [EnumMember(Value = "Cocurricular Programs")]
        CocurricularPrograms,
        [EnumMember(Value = "College Preparatory")]
        CollegePreparatory,
        [EnumMember(Value = "Community Service Program")]
        CommunityServiceProgram,
        [EnumMember(Value = "Community/Junior College Education Program")]
        CommunitJuniorCollegeEducationProgram,
        [EnumMember(Value = "Compensatory Services for Disadvantaged Students")]
        CompensatoryServicesForDisadvantagedStudents,
        [EnumMember(Value = "Counseling Services")]
        CounselingServices,
        [EnumMember(Value = "English as a Second Language (ESL)")]
        EnglishAsASecondLanguageESL,
        [EnumMember(Value = "Even Start")]
        EvenStart,
        [EnumMember(Value = "Extended Day/Child Care Services")]
        ExtendedDayChildCareServices,
        [EnumMember(Value = "Gifted and Talented")]
        GiftedAndTalented,
        [EnumMember(Value = "Head Start")]
        HeadStart,
        [EnumMember(Value = "Health Services Program")]
        HealthServicesProgram,
        [EnumMember(Value = "High School Equivalency Program (HSEP)")]
        HighSchoolEquivalencyProgramHSEP,
        [EnumMember(Value = "IDEA")]
        IDEA,
        [EnumMember(Value = "Immigrant Education")]
        ImmigrantEducation,
        [EnumMember(Value = "Indian Education")]
        IndianEducation,
        [EnumMember(Value = "International Baccalaureate")]
        InternationalBaccalaureate,
        [EnumMember(Value = "Library/Media Services Program")]
        LibraryMediaServicesProgram,
        [EnumMember(Value = "Magnet/Special Program Emphasis")]
        MagnetSpecialProgramEmphasis,
        [EnumMember(Value = "Migrant Education")]
        MigrantEducation,
        [EnumMember(Value = "Neglected and Delinquent Program")]
        NeglectedAndDelinquentProgram,
        [EnumMember(Value = "Optional Flexible School Day Program (OFSDP)")]
        OptionalFlexibleSchoolDayProgramOFSDP,
        [EnumMember(Value = "Other")]
        Other,
        [EnumMember(Value = "Regular Education")]
        RegularEducation,
        [EnumMember(Value = "Remedial Education")]
        RemedialEducation,
        [EnumMember(Value = "Section 504 Placement")]
        Section504Placement,
        [EnumMember(Value = "Service Learning")]
        ServiceLearning,
        [EnumMember(Value = "Special Education")]
        SpecialEducation,
        [EnumMember(Value = "Student Retention/Dropout Prevention")]
        StudentRetentionDropoutPrevention,
        [EnumMember(Value = "Substance Abuse Education/Prevention")]
        SubstanceAbuseEducationPrevention,
        [EnumMember(Value = "Teacher Professional Development/Mentoring")]
        TeacherProfessionalDevelopmentMentoring,
        [EnumMember(Value = "Technical Preparatory")]
        TechnicalPreparatory,
        [EnumMember(Value = "Title I Part A")]
        TitleIPartA,
        [EnumMember(Value = "Vocational Education")]
        VocationalEducation
    }
}