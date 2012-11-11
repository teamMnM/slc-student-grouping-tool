using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlcClient.Constants
{
    public class Student
    {
        public const string ALL = "students/";
        public const string STUDENT_ASSESSMENT_ASSOCIATION_ASSESSMENTS = "students/{studentId}/studentAssessments/assessments";
        public const string STUDENT_SECTION_ASSOCIATION_SECTIONS = "students/{studentId}/studentSectionAssociations/sections";
    }
}