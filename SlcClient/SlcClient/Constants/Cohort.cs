using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlcClient.Constants
{
    public class Cohort
    {
        public const string ALL = "cohorts/";
        public const string STUDENT_COHORT_ASSOCIATION = "studentCohortAssociations/";
        public const string GET_STUDENT_COHORT_ASSOCIATION = "cohorts/{cohortId}/studentCohortAssociations/";
        public const string STAFF_COHORT_ASSOCIATION = "staffCohortAssociations/";
        public const string STUDENT_COHORT_ASSOCIATION_STUDENTS = "cohorts/{cohortId}/studentCohortAssociations/students";
        public const string COHORT_CUSTOM = "cohorts/{cohortId}/custom";
    }
}