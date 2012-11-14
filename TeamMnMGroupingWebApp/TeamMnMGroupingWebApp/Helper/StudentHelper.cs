using Newtonsoft.Json;
using SlcClient.Entities;
using SlcClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TeamMnMGroupingWebApp.Models;

namespace TeamMnMGroupingWebApp.Helper
{
    public class StudentHelper
    {
        public static async Task<StudentDisplayObject> GetStudentDisplayObject(StudentService ss, Student student)
        {
            var sections = GetSectionsByStudentId(ss, student.id);
            var assessments = GetStudentAssessmentsByStudentId(ss, student.id);
            var academicRecords = GetAllStudentsAcademicRecords(ss);

            await Task.WhenAll(sections, assessments, academicRecords);

            var result = MapStudentToStudentDisplayObject(student, sections.Result, assessments.Result, academicRecords.Result);

            return result;
        }

        [OutputCache(Duration=1200, VaryByParam="none")]
        public static async Task<IEnumerable<StudentAcademicRecord>> GetAllStudentsAcademicRecords(StudentService ss)
        {
            var records = await ss.GetAllStudentsAcademicRecords();
            return records;
        }

        private static async Task<IEnumerable<Section>> GetSectionsByStudentId(StudentService cs, string studentId)
        {
            var result = await cs.GetStudentSectionsByStudentId(studentId);
            return result;
        }

        private static async Task<IEnumerable<Assessment>> GetStudentAssessmentsByStudentId(StudentService cs, string studentId)
        {
            var result = await cs.GetStudentAssessmentsByStudentId(studentId);
            return result;            
        }

        public static StudentDisplayObject MapStudentToStudentDisplayObject(Student student, IEnumerable<Section> sections, IEnumerable<Assessment> assessments, IEnumerable<StudentAcademicRecord> academicRecords)
        {
            
            var newStudent = new StudentDisplayObject();
            try
            {
                newStudent.id = student.id;
                newStudent.name = string.Format("{0} {1}", student.name.firstName, student.name.lastSurName);
                newStudent.sections = from s in sections select s.id;
                newStudent.disabilities = from d in student.disabilities select FilterHelper.GetEnumDescription(d.disability).title;

                //sometime there's no learning style data
                if (student.learningStyles != null)
                {
                    newStudent.auditoryLearning = student.learningStyles.auditoryLearning;
                    newStudent.tactileLearning = student.learningStyles.tactileLearning;
                    newStudent.visualLearning = student.learningStyles.visualLearning;
                }
                
                newStudent.birthDate = student.birthData.birthDate;
                newStudent.profileThumbnail = student.profileThumbnail;
                newStudent.race = student.race;
                newStudent.schoolFoodServicesEligiblity = student.schoolFoodServicesEligiblity;
                newStudent.section504Disablities = student.section504Disablities;
                newStudent.studentCharacteristics = from sc in student.studentCharacteristics select FilterHelper.GetEnumDescription(sc.characteristic).title;

                //get the gpa
                var studentAcademicRecord = academicRecords.FirstOrDefault(a => a.studentId == student.id);
                newStudent.cumulativeGradePointAverage = studentAcademicRecord != null ? studentAcademicRecord.cumulativeGradePointAverage : 0;

                newStudent.assessments = assessments;
            }
            catch (Exception e)
            {
                //log something here
            }
            
            return newStudent;
        }

        private static IEnumerable<SectionDisplayObject> MapSectionToSectionDisplayObject(IEnumerable<Section> sections)
        {
            var newSections = new List<SectionDisplayObject>();
            foreach (var s in sections)
            {
                var newSection = new SectionDisplayObject();
                newSection.courseTitle = s.course.courseTitle;
                newSection.courseDescription = s.course.courseDescription;
                newSection.courseLevel = s.course.courseLevel;
                newSection.subjectArea = s.course.subjectArea;
                newSections.Add(newSection);
            }
            return newSections;
        }
    }
}