using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SlcClient.Enum
{
    [DataContract]
    public enum StudentCharacteristicType
    {
        [EnumMember(Value = "Displaced Homemaker")]
        DisplacedHomemaker,
        [EnumMember(Value = "Foster Care")]
        FosterCare,
        [EnumMember(Value = "Homeless")]
        Homeless,
        [EnumMember(Value = "Immigrant")]
        Immigrant,
        [EnumMember(Value = "Migratory")]
        Migratory,
        [EnumMember(Value = "Parent in Military")]
        ParentInMilitary,
        [EnumMember(Value = "Pregnant")]
        Pregnant,
        [EnumMember(Value = "Single Parent")]
        SingleParent,
        [EnumMember(Value = "Unaccompanied Youth")]
        UnaccompaniedYouth,
        [EnumMember(Value = "Unschooled Asylee")]
        UnschooledAsylee,
        [EnumMember(Value = "Unschooled Refugee")]
        UnschooledRefugee
    }
}