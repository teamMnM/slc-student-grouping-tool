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
        static string[] logicalOperators = { "=", ">", "<", "<=", ">=" };
        static string[] equalOperator = { "equals" };

        static IEnumerable<FilterValue> trueFalse = new List<FilterValue> {
                                                new FilterValue () { id = "true", title = "true" },
                                                new FilterValue () { id = "false", title = "false"}
                };

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

        static IEnumerable<FilterValue> studentCharacteristicTypes = from StudentCharacteristicType s in Enum.GetValues(typeof(StudentCharacteristicType))
                                                   select GetEnumDescription(s);

        public static IEnumerable<Filter> InitializeFilters()
        {
            var filters = new List<Filter>();

            //enum filters
            var disabilities = new Filter { attributeId = "disabilities", attributeName = "Disabilities", operators = containsOperator, values = disabilityTypes };
            var gradeLevels = new Filter { attributeId = "gradeLevel", attributeName = "Grade Level", operators = containsOperator, values = gradeLevelTypes };
            var homeLanguageItems = new Filter { attributeId = "homeLanguages", attributeName = "Home Languages", operators = containsOperator, values = languageItemTypes };
            var languageItems = new Filter { attributeId = "languages", attributeName = "Languages", operators = containsOperator, values = languageItemTypes };
            var oldEthnicities = new Filter { attributeId = "oldEthnicity", attributeName = "Old Ethnicity", operators = containsOperator, values = oldEthnicityTypes };
            var raceItems = new Filter { attributeId = "race", attributeName = "Race", operators = containsOperator, values = raceItemTypes };
            var schoolFoodServicesEligibilities = new Filter { attributeId = "schoolFoodServicesEligiblity", attributeName = "School Food Services Eligibility", operators = containsOperator, values = schoolFoodServicesEligibilityTypes };
            var section504DisabilityItems = new Filter { attributeId = "section504Disablities", attributeName = "Section 504 Disabilities", operators = containsOperator, values = section504DisabilityItemTypes };
            var sex = new Filter { attributeId = "sex", attributeName = "Gender", operators = containsOperator, values = sexTypes };
            var studentCharacteristics = new Filter { attributeId = "studentCharacteristics", attributeName = "Student Characteristics", operators = containsOperator, values = studentCharacteristicTypes }; 

            var birthDate = new Filter { attributeId = "birthDate", attributeName = "Birth Date", operators = logicalOperators };
            var economicDisadvantaged = new Filter { attributeId = "economicDisadvantaged", attributeName = "Economic Disadvantaged", operators = equalOperator, values = trueFalse };

            var hispanicLatinoEthnicity = new Filter
            {
                attributeId = "hispanicLatinoEthnicity",
                attributeName = "Hispanic Latino Ethnicity",
                operators = equalOperator,
                values = trueFalse
            };

            var auditoryLearning = new Filter
            {
                attributeId = "auditoryLearning",
                attributeName = "Auditory Learning",
                operators = logicalOperators
            };

            var tactileLearning = new Filter
            {
                attributeId = "tactileLearning",
                attributeName = "Tactile Learning",
                operators = logicalOperators
            };

            var visualLearning = new Filter
            {
                attributeId = "visualLearning",
                attributeName = "Visual Learning",
                operators = logicalOperators
            };

            var limitedEnglishProficiency = new Filter
            {
                attributeId = "limitedEnglishProficiency",
                attributeName = "Limited English Proficiency",
                operators = equalOperator,
                values = trueFalse
            };


            return new List<Filter>() { disabilities, gradeLevels, languageItems, homeLanguageItems, 
                oldEthnicities, raceItems, schoolFoodServicesEligibilities, section504DisabilityItems, sex,
                studentCharacteristics, birthDate, economicDisadvantaged, hispanicLatinoEthnicity, 
                auditoryLearning, tactileLearning, visualLearning, limitedEnglishProficiency
            };
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