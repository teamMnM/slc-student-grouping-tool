using SlcClient.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using TeamMnMGroupingWebApp.Models;

namespace TeamMnMGroupingWebApp.Helper
{
    public class FilterHelper
    {
        static string[] containsOperator = { "contains" };

        static IEnumerable<FilterValue> disabilityTypes = from DisabilityType s in Enum.GetValues(typeof(DisabilityType))
                                                     select GetEnumDescription(s);

        static IEnumerable<FilterValue> gradeLevelTypes = from GradeLevelType s in Enum.GetValues(typeof(GradeLevelType))
                                                     select GetEnumDescription(s);

        static IEnumerable<FilterValue> languageItemTypes = from LanguageItemType s in Enum.GetValues(typeof(LanguageItemType))
                                                       select GetEnumDescription(s);

        static IEnumerable<FilterValue> oldEthnicityTypes = from OldEthnicityType s in Enum.GetValues(typeof(OldEthnicityType))
                                                       select GetEnumDescription(s);

        static IEnumerable<FilterValue> raceItemTypes = from RaceItemType s in Enum.GetValues(typeof(RaceItemType))
                                                   select GetEnumDescription(s);

        static IEnumerable<FilterValue> schoolFoodServicesEligibilityTypes = from SchoolFoodServicesEligibilityType s in Enum.GetValues(typeof(SchoolFoodServicesEligibilityType))
                                                                        select GetEnumDescription(s);

        static IEnumerable<FilterValue> section504DisabilityItemTypes = from Section504DisabilityItemType s in Enum.GetValues(typeof(Section504DisabilityItemType))
                                                                   select GetEnumDescription(s);

        static IEnumerable<FilterValue> sexTypes = from SexType s in Enum.GetValues(typeof(SexType))
                                              select GetEnumDescription(s);

        public static IEnumerable<Filter> InitializeFilters()
        {
            var filters = new List<Filter>();

            var disabilities = new Filter { attributeId = "disabilities", attributeName = "Disabilities", operators = containsOperator, values = disabilityTypes };
            var gradeLevels = new Filter { attributeId = "gradeLevel", attributeName = "Grade Level", operators = containsOperator, values = gradeLevelTypes };
            var languageItems = new Filter { attributeId = "gradeLevel", attributeName = "Grade Level", operators = containsOperator, values = languageItemTypes };
            var oldEthnicities = new Filter { attributeId = "gradeLevel", attributeName = "Grade Level", operators = containsOperator, values = oldEthnicityTypes };
            var raceItems = new Filter { attributeId = "gradeLevel", attributeName = "Grade Level", operators = containsOperator, values = raceItemTypes };
            var schoolFoodServicesEligibilities = new Filter { attributeId = "gradeLevel", attributeName = "Grade Level", operators = containsOperator, values = schoolFoodServicesEligibilityTypes };
            var section504DisabilityItems = new Filter { attributeId = "gradeLevel", attributeName = "Grade Level", operators = containsOperator, values = section504DisabilityItemTypes };
            var sex = new Filter { attributeId = "gradeLevel", attributeName = "Grade Level", operators = containsOperator, values = sexTypes };

            return new List<Filter>() { disabilities, gradeLevels, languageItems, oldEthnicities, raceItems, schoolFoodServicesEligibilities, section504DisabilityItems, sex };
        }

        public static FilterValue GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return new FilterValue { id = attributes[0].Description, title = attributes[0].Description };
            else
                return new FilterValue { id = value.ToString(), title = value.ToString() };
        }
    }
}