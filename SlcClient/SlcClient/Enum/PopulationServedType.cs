using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum PopulationServedType
    {
        [EnumMember(Value = "Regular Students")]
        RegularStudents,
        [EnumMember(Value = "Bilingual Students")]
        BilingualStudents,
        [EnumMember(Value = "Compensatory/Remedial Education Students")]
        CompensatoryRemedialEducationStudents,
        [EnumMember(Value = "Gifted and Talented Students")]
        GiftedAndTalentedStudents,
        [EnumMember(Value = "Career and Technical Education Students")]
        CareerAndTechnicalEducationStudents,
        [EnumMember(Value = "Special Education Students")]
        SpecialEducationStudents,
        [EnumMember(Value = "ESL Students")]
        ESLStudents,
        [EnumMember(Value = "Adult Basic Education Students")]
        AdultBasicEducationStudents,
        [EnumMember(Value = "Honors Students")]
        HonorsStudents,
        [EnumMember(Value = "Migrant Students")]
        MigrantStudents
    }
}