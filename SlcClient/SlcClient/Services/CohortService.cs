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
    public class CohortService : ISlcService<Cohort>
    {
        private SlcHttpClient _client;

        public CohortService(string token)
        {
            _client = new SlcHttpClient(token);
        }

        /// <summary>
        /// Get all cohorts that the current user has access to
        /// </summary>
        /// <returns>all cohorts the current user has access to</returns>
        public async Task<IEnumerable<Cohort>> GetAll()
        {
            try
            {
                var response = await _client.GetDataString(Constants.Cohort.ALL);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var cohorts = JsonConvert.DeserializeObject<IEnumerable<Cohort>>(content);
                    return cohorts; //successful request, return list of cohorts
                }
                return new List<Cohort>(); //problem with getting cohorts back, return empty list
            }
            catch (Exception e)
            {
                //log here
                throw;
            }           
        }

        /// <summary>
        /// Get a cohort by Id
        /// </summary>
        /// <param name="id">Id of the cohort to get</param>
        /// <returns>the requested cohort</returns>
        public async Task<Cohort> GetById(string id)
        {
            var response = await _client.GetDataString(Constants.Cohort.ALL + id);
            var content = await response.Content.ReadAsStringAsync();
            var cohort = JsonConvert.DeserializeObject<Cohort>(content);
            return cohort;
        }

        /// <summary>
        /// Create a cohort
        /// </summary>
        /// <param name="obj">the cohort to create</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Create(Cohort obj)
        {
            var response = await _client.PostData(Constants.Cohort.ALL, obj);
            //var content = await response.Content.ReadAsStringAsync();
            return response;
        }

        /// <summary>
        /// Update a cohort by ID
        /// </summary>
        /// <param name="obj">the cohort to update</param>
        /// <returns>status of the request</returns>
        public async Task<HttpStatusCode> Update(Cohort obj)
        {
            var response = await _client.PutData(Constants.Cohort.ALL + obj.id, obj);
            return response.StatusCode;
        }

        /// <summary>
        /// Delete a cohort by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>status of the request</returns>
        public async Task<HttpStatusCode> DeleteById(string id)
        {
            var response = await _client.DeleteData(Constants.Cohort.ALL, id);
            return response.StatusCode;
        }

        /// <summary>
        /// Get the students that belong to the cohort in context
        /// </summary>
        /// <param name="id">cohortId</param>
        /// <returns>list of students in cohort with id = cohortId</returns>
        public async Task<IEnumerable<Student>> GetStudentsByCohortId(string id)
        {
            var strEndPoint = Constants.Cohort.STUDENT_COHORT_ASSOCIATION_STUDENTS.Replace("{cohortId}", id);
            var response = await _client.GetDataString(strEndPoint);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var students = JsonConvert.DeserializeObject<IEnumerable<Student>>(content);
                return students;
            }
            return new List<Student>();
        }

        /// <summary>
        /// Get the student cohort association
        /// </summary>
        /// <param name="id">cohortId</param>
        /// <returns>list of studentCohortAssociations in a cohort with id = cohortId</returns>
        public async Task<IEnumerable<StudentCohortAssociation>> GetStudentCohortAssociationsByCohortId(string id)
        {
            var strEndPoint = Constants.Cohort.GET_STUDENT_COHORT_ASSOCIATION.Replace("{cohortId}", id);
            var response = await _client.GetDataString(strEndPoint);
            var content = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<IEnumerable<StudentCohortAssociation>>(content);
            return list;
        }

        /// <summary>
        /// Associate a student with a cohort
        /// </summary>
        /// <param name="obj">StudentCohortAssociation requires: cohortId, studentId, beginDate</param>
        /// <returns>status code of the request</returns>
        public async Task<HttpStatusCode> CreateStudentCohortAssociation(StudentCohortAssociation obj)
        {
            var response = await _client.PostData(Constants.Cohort.STUDENT_COHORT_ASSOCIATION, obj);
            return response.StatusCode;
        }

        /// <summary>
        /// Remove a student from a cohort
        /// </summary>
        /// <param name="id">the association ID</param>
        /// <returns>status code of the request</returns>
        public async Task<HttpStatusCode> DeleteStudentCohortAssociationById(string id)
        {
            var response = await _client.DeleteData(Constants.Cohort.STUDENT_COHORT_ASSOCIATION, id);
            return response.StatusCode;
        }

        /// <summary>
        /// Associate a staff with a cohort
        /// </summary>
        /// <param name="obj">StaffCohortAssociation requires: cohortId, staffId, beginDate</param>
        /// <returns>status code of the request</returns>
        public async Task<HttpStatusCode> CreateStaffCohortAssociation(StaffCohortAssociation obj)
        {
            var response = await _client.PostData(Constants.Cohort.STUDENT_COHORT_ASSOCIATION, obj);           
            return response.StatusCode;
        }

        /// <summary>
        /// Remove a staff from a cohort
        /// </summary>
        /// <param name="id">the association ID</param>
        /// <returns>status code of the request</returns>
        public async Task<HttpStatusCode> DeleteStaffCohortAssociationById(string id)
        {
            var result = await _client.DeleteData(Constants.Cohort.STUDENT_COHORT_ASSOCIATION, id);
            return result.StatusCode;
        }

        /// <summary>
        /// Get a cohort custom entity
        /// </summary>
        /// <param name="id">Id of the cohort to get</param>
        /// <returns>everything in the custom entity as string format</returns>
        public async Task<string> GetCustomById(string id)
        {
            try
            {
                var strEndPoint = Constants.Cohort.COHORT_CUSTOM.Replace("{cohortId}", id);
                var response = await _client.GetDataString(strEndPoint);
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return ""; //custom doesn't exist yet
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception e)
            {
                //SLC throws a 404 if custom doesn't exist yet
                return "";
            }            
        }

        /// <summary>
        /// Create a cohort custom
        /// </summary>
        /// <param name="obj">the cohort to create</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> CreateCohortCustom(string id, string param)
        {
            var strEndPoint = Constants.Cohort.COHORT_CUSTOM.Replace("{cohortId}", id);
            var response = await _client.PostData(strEndPoint, param);
            //var content = await response.Content.ReadAsStringAsync();
            return response;
        }

        /// <summary>
        /// Update the cohort custom entity
        /// </summary>
        /// <param name="obj">the cohort to update</param>
        /// <returns></returns>
        public async Task<HttpStatusCode> UpdateCohortCustom(string id, string param)
        {
            try
            {
                var strEndPoint = Constants.Cohort.COHORT_CUSTOM.Replace("{cohortId}", id);
                var response = await _client.PutData(strEndPoint, param);
                return response.StatusCode;
            }
            catch (Exception e)
            {
                //SLC throws a 404 if custom doesn't exist yet
                return HttpStatusCode.NotFound;
            }            
        }
    }
}
