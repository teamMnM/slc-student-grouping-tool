using SlcClient.Entities;
using SlcClient.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamMnMGroupingWebApp.Models
{
    public class StudentDisplayObject
    {
        public string id { get; set; }

        /// <summary>
        /// first name + last name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// list of disability names
        /// </summary>
        public IEnumerable<string> disabilities { get; set; }

        public string displacementStatus { get; set; }

        public bool economicDisadvantaged { get; set; }

        public string gradeLevel { get; set; }

        public bool hispanicLatinoEthnicity { get; set; }

        public IEnumerable<string> homeLanguages { get; set; }

        public IEnumerable<string> languages { get; set; }

        public string limitedEnglishProficiency { get; set; }

        public string oldEthnicity { get; set; }

        public IEnumerable<string> otherName { get; set; }

        public string sex { get; set; }

        public IEnumerable<string> studentCharacteristics { get; set; }

        public IEnumerable<string> studentIndicators { get; set; }

        public IEnumerable<Telephone> telephones { get; set; }

        public DateTime birthDate { get; set; }

        public LearningStyles learningStyles { get; set; }

        public double auditoryLearning { get; set; }

        public double tactileLearning { get; set; }

        public double visualLearning { get; set; }

        public string profileThumbnail { get; set; }

        public IEnumerable<string> race { get; set; }

        public string schoolFoodServicesEligiblity { get; set; }

        public IEnumerable<string> section504Disablities { get; set; }

        /// <summary>
        /// GPA
        /// </summary>
        public double cumulativeGradePointAverage { get; set; }

        /// <summary>
        /// list of sectionId's the students belong to
        /// </summary>
        public IEnumerable<string> sections { get; set; }

        /// <summary>
        /// assessments that the student in context has taken
        /// </summary>
        public IEnumerable<Assessment> assessments { get; set; }
    }
}