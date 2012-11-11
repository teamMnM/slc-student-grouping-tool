using Newtonsoft.Json;
using SlcClient.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SlcClient.Services
{
    public class StudentService : ISlcService<Student>
    {
        private SlcHttpClient _client;

        public StudentService(string token)
        {
            _client = new SlcHttpClient(token);
        }

        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Student>> GetAll()
        {
            try
            {
                var response = await _client.GetDataString(Constants.Student.ALL);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var students = JsonConvert.DeserializeObject<IEnumerable<Student>>(content);
                    return students; //successful request, return list of students
                }
                return new List<Student>(); //problem with getting students back return empty list
            }
            catch (Exception e)
            {
                //log here
                throw;
            }           
        }

        /// <summary>
        /// Get the student object by id
        /// </summary>
        /// <param name="id">id of the student</param>
        /// <returns></returns>
        public async Task<Student> GetById(string id)
        {
            var response = await _client.GetDataString(Constants.Student.ALL + id);
            var content = await response.Content.ReadAsStringAsync();
            var student = JsonConvert.DeserializeObject<Student>(content);
            return student;
        }

        /// <summary>
        /// Get a list of sections the student is in
        /// </summary>
        /// <param name="studentId">the id of the student</param>
        /// <returns></returns>
        public async Task<IEnumerable<Section>> GetStudentSectionsByStudentId(string studentId)
        {
            var strEndPoint = Constants.Student.STUDENT_SECTION_ASSOCIATION_SECTIONS.Replace("{studentId}", studentId);
            var response = await _client.GetDataString(strEndPoint);
 
            //forbidden access is sometimes returned
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var sections = JsonConvert.DeserializeObject<IEnumerable<Section>>(content);
                return sections;
            }
            
            return new List<Section>();
        }

        /// <summary>
        /// Get a list of assessments the student is associated with
        /// </summary>
        /// <param name="studentId">the id of the student</param>
        /// <returns></returns>
        public async Task<IEnumerable<Assessment>> GetStudentAssessmentsByStudentId(string studentId)
        {
            var strEndPoint = Constants.Student.STUDENT_ASSESSMENT_ASSOCIATION_ASSESSMENTS.Replace("{studentId}", studentId);
            var response = await _client.GetDataString(strEndPoint);

            //forbidden access is sometimes returned
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var assessments = JsonConvert.DeserializeObject<IEnumerable<Assessment>>(content);
                return assessments;
            }

            return new List<Assessment>();
        }

        public async Task<HttpResponseMessage> Create(Student obj)
        {
            throw new NotImplementedException("Creating student is not allowed");
        }

        public async Task<HttpStatusCode> Update(Student obj)
        {
            throw new NotImplementedException("Updating student is not allowed");
        }

        public async Task<HttpStatusCode> DeleteById(string id)
        {
            throw new NotImplementedException("Deleting student is not allowed");
        }
    }
}
