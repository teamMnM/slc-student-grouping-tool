using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using SlcClient.Entities;
using SlcClient.Services;
using TeamMnMGroupingWebApp.Controllers;
using TeamMnMGroupingWebApp.Models;
using TeamMnMGroupingWebApp.Helper;
using System.Net.Http;
using System.Web.Caching;
using System.Text;
using System.Configuration;

namespace TeamMnMGroupingWebApp.Helper
{
    public class CohortActionHelper
    {
        internal string CURRENT_ED_ORG_ID = ConfigurationManager.AppSettings["CurrentEdgOrgId"]; //there's no data from SLC about the current user Ed Org, temporarily using a constant value for each environment


        /// <summary>
        /// Create new student cohort associations
        /// </summary>
        /// <param name="obj">cohort to create</param>
        /// <param name="cs">cohort service</param>
        /// <returns>result of this action</returns>
        public static Task<IEnumerable<ActionResponseResult>> GetNewStudentCohortAssociations(CohortActionObject obj, CohortService cs)
        {
            Task<IEnumerable<ActionResponseResult>> newStudentsAssociations;
            if (obj.studentsToCreate != null && obj.studentsToCreate.Count() > 0)
                newStudentsAssociations = CohortActionHelper.CreateMultipleStudentCohortAssociations(cs, obj.cohort.id, obj.studentsToCreate);
            else
                newStudentsAssociations = null;
            return newStudentsAssociations;
        }
        
        /// <summary>
        /// Create multiple StudentCohortAssociation object
        /// </summary>
        /// <param name="cs">the service to use for this action</param>
        /// <param name="associations">the StudentCohortAssociations to create</param>
        /// <returns>list of results</returns>
        public static async Task<IEnumerable<ActionResponseResult>> CreateMultipleStudentCohortAssociations(CohortService cs, string cId, IEnumerable<string> sl)
        {
            var result = await Task.WhenAll(from s in sl select CreateOneStudentCohortAssociation(cs, cId, s));
            return result;
        }

        /// <summary>
        /// Create a StudentCohortAssociation object
        /// </summary>
        /// <param name="cs">the service to use for this action</param>
        /// <param name="associations">the StudentCohortAssociation to create</param>
        /// <returns>result of this action</returns>
        public static async Task<ActionResponseResult> CreateOneStudentCohortAssociation(CohortService cs, string cId, string sId)
        {
            var a = new StudentCohortAssociation { cohortId = cId, studentId = sId, beginDate = DateTime.Now };
            var result = await cs.CreateStudentCohortAssociation(a);

            return GlobalHelper.GetActionResponseResult(sId, result, HttpStatusCode.Created);
            //return new ActionResponseResult { data = sId, status = result.StatusCode, message = result.Content.ReadAsStringAsync().Result };
        }

        /// <summary>
        /// Delete multiple StudentCohortAssociation objects
        /// </summary>
        /// <param name="cs">the service to use for this action</param>
        /// <param name="associations">the StudentCohortAssociations to delete</param>
        /// <returns>result of this action</returns>
        public static async Task<IEnumerable<ActionResponseResult>> DeleteMultipleStudentCohortAssociations(CohortService cs, IEnumerable<StudentCohortAssociation> associations)
        {
            var result = await Task.WhenAll(from a in associations select DeleteOneStudentCohortAssociation(cs, a));
            return result;
        }

        /// <summary>
        /// Delete a StudentCohortAssociation object
        /// </summary>
        /// <param name="cs">service to use for this action</param>
        /// <param name="association">the StudentCohortAssociation to delete</param>
        /// <returns>result of this action</returns>
        public static async Task<ActionResponseResult> DeleteOneStudentCohortAssociation(CohortService cs, StudentCohortAssociation association)
        {
            var response = await cs.DeleteStudentCohortAssociationById(association.id);
            return GlobalHelper.GetActionResponseResult(association.studentId, response, HttpStatusCode.NoContent);
        }     

        /// <summary>
        /// Get the list of studentIds that the system failed to create associations for
        /// </summary>
        /// <param name="result">the result object to populate result to</param>
        /// <param name="associations">the response from the service for the attempted creation</param>
        public static void DetermineFailedToCreateFor(Result result, IEnumerable<ActionResponseResult> associations)
        {
            //determine if all the associations were created successfully
            var assocationSuccess = associations.All(a => a.status == HttpStatusCode.Created);
            if (result.completedSuccessfully) //if overal success is true and set to to false if this action is false, else overal success is still false
                result.completedSuccessfully = assocationSuccess;

            if (!result.completedSuccessfully)
                result.partialCreateSuccess = associations.Any(a => a.status == HttpStatusCode.Created);
            else
                result.partialCreateSuccess = false;

            result.failToCreateAssocations = from r in associations where r.status != HttpStatusCode.Created select r;
        }

        /// <summary>
        /// Get the list of studentIds that the system failed to delete associations for
        /// </summary>
        /// <param name="result">the result object to populate result to</param>
        /// <param name="associations">the response from the service for the attempted deletion</param>
        public static void DetermineFailedToDeleteFor(Result result, IEnumerable<ActionResponseResult> associations)
        {
            //determine if all the associations were created successfully
            var assocationSuccess = associations.All(a => a.status == HttpStatusCode.NoContent);
            if (result.completedSuccessfully) //if overal success is true and set to to false if this action is false, else overal success is still false
                result.completedSuccessfully = assocationSuccess;

            if (!result.completedSuccessfully)
                result.partialDeleteSuccess = associations.Any(a => a.status == HttpStatusCode.NoContent);
            else
                result.partialDeleteSuccess = false;

            result.failToDeleteAssociations = from r in associations where r.status != HttpStatusCode.NoContent select r;
        }

        /// <summary>
        /// Update the Result object after custom has been updated through SLC
        /// </summary>
        /// <param name="cohortResult">Result object to update</param>
        /// <param name="cohortCustom">the SLC response message</param>
        /// <param name="successStatus">HttpStatusCode that indicates a successful response</param>
        public static void ProcessCustomResult(Result cohortResult, Task<HttpResponseMessage> cohortCustom, HttpStatusCode successStatus)
        {
            var customResult = GlobalHelper.GetActionResponseResult(cohortResult.objectId, cohortCustom.Result, successStatus);
            if (cohortCustom.Result.StatusCode != successStatus)
                cohortResult.completedSuccessfully = false;

            cohortResult.customActionResult = customResult;
        }

        public static Task<HttpResponseMessage> UpdateCustom(CohortActionObject obj, CohortService cs)
        {
            var custom = obj.custom;
            if (custom == null) custom = new CohortCustom { lastModifiedDate = DateTime.UtcNow };
            else custom.lastModifiedDate = DateTime.UtcNow;
            var cohortCustom = cs.UpdateCohortCustom(obj.cohort.id, JsonConvert.SerializeObject(custom));
            return cohortCustom;
        }

        public static async Task<Task<IEnumerable<ActionResponseResult>>> DeleteStudentCohortAssocations(CohortActionObject obj, CohortService cs)
        {
            Task<IEnumerable<ActionResponseResult>> removeStudents;
            //Get a list of the current studentCohortAssociations so that we have the ids to delete them from group
            var currentStudentCohortAssociation = await cs.GetStudentCohortAssociationsByCohortId(obj.cohort.id);
            //get the studentCohortAssociationId for students to delete
            var associationToDelete = (from s in obj.studentsToDelete
                                       select (currentStudentCohortAssociation.FirstOrDefault(csca => csca.studentId == s)));
            //delete the studentCohortAssociation
            removeStudents = CohortActionHelper.DeleteMultipleStudentCohortAssociations(cs, associationToDelete);
            return removeStudents;
        }
    }
}