using SlcClient.Entities;
using SlcClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TeamMnMGroupingWebApp.Controllers
{
    [Authorize]
    public class CohortsController : ApiController
    {
        private CohortService _cohortService = new CohortService("abc");

        /// <summary>
        /// Get all cohort
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public IEnumerable<Cohort> Get()
        {
            var cohorts = _cohortService.GetAll().Result;
            return cohorts;
        }

        /// <summary>
        /// Get cohort by id
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string id)
        {
            var cohort = _cohortService.GetById(id).Result;
            if (cohort == null)
            {
                var notFoundMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
                return notFoundMessage;
            }
            var newMessage = new HttpResponseMessage(HttpStatusCode.OK);
            
            return newMessage;
        }

        /// <summary>
        /// Create a cohort
        /// https://api.sandbox.slcedu.org/api/rest/v1/cohorts
        /// Validation failed: ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=cohortIdentifier, fieldValue=, expectedTypes=[STRING]] 
        /// ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=cohortType, fieldValue=, expectedTypes=[TOKEN]] 
        /// ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=educationOrgId, fieldValue=, expectedTypes=[REFERENCE]]",
        /// cohortType: Academic Intervention, Attendance Intervention, Discipline Intervention, Classroom Pullout, Extracurricular Activity, Field Trip, Principal Watch List, 
        /// Counselor List, In-school Suspension, Study Hall, Other
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cohort">Required properties: cohortIdentifier, cohortType, and educationOrgId</param>
        /// <returns></returns>
        public HttpResponseMessage Post(Cohort obj)
        {
            try
            {
                var result = _cohortService.Create(obj).Result;
                return result;
            }
            catch(Exception e)
            {
                var notFoundMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                return notFoundMessage;
            }
        }
    }
}
