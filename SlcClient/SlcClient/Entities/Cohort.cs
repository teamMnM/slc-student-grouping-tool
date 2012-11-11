using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SlcClient.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SlcClient.Entities
{
    public class Cohort
    {
        public string id { get; set; }

        [Required]
        public string educationOrgId { get; set; }

        /// <summary>
        /// The academic subject associated with an academic intervention.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public AcademicSubjectType academicSubject { get; set; }

        /// <summary>
        /// The description of he cohort and its purpose.
        /// </summary>
        [StringLength(1024)]
        public string cohortDescription { get; set; }

        /// <summary>
        /// The name or ID for the cohort.
        /// </summary>
        [StringLength(20)]
        public string cohortIdentifier { get; set; }

        //District, School, Classroom, Teacher, Principal, Counselor, Statewide
        [JsonConverter(typeof(StringEnumConverter))]
        public CohortScopeType cohortScope { get; set; }

        //Academic Intervention, Attendance Intervention, Discipline Intervention, Classroom Pullout, Extracurricular Activity, Field Trip, 
        //Principal Watch List, Counselor List, In-school Suspension, Study Hall, Other
        /// <summary>
        /// The type of cohort (e.g., academic intervention, classroom breakout)
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CohortType cohortType { get; set; }

        ///// <summary>
        ///// The education organization associated with and owner of the cohort.
        ///// </summary>
        //public EducationOrganization educationOrg { get; set; }

        ///// <summary>
        ///// The (optional) program associated with this cohort (e.g., special education)
        ///// </summary>
        //public List<Program> programs { get; set; }
    }
}