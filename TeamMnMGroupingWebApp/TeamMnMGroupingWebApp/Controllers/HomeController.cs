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

namespace TeamMnMGroupingWebApp.Controllers
{
    [AsyncTimeout(5000)]
    [HandleError(ExceptionType = typeof(TimeoutException), View = "Timeout")]
    [HandleError(ExceptionType = typeof(NullReferenceException), View = "Error")]
    [HandleError(ExceptionType = typeof(HttpRequestException), View = "PermissionError")]
    public class HomeController : BaseController
    {
        const string MAIN = "/Home/Index2";
        const string SLC_USER_SESSION = "slc_user";

        public void Index()
        {
            if (Session["access_token"] == null)
            {
                GetToken(MAIN);
            }
            else
            {
                // We have an access token in session, let's redirect to app main page.
                Response.Redirect(MAIN);
            }
        }

        [OutputCache(Duration = 1200, VaryByParam = "none")]
        public ActionResult Index2()
        {
            return View("Index");
        }

        /// <summary>
        /// AJAX to this method to create brand new groups with students
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<Result> CreateGroup(CohortActionObject obj)
        {
            try
            {
                var cs = new CohortService(Session["access_token"].ToString());
                //1) create the cohort first
                var cohortResult = await CreateCohort(cs, obj.cohort);
                
                //if cohort was created successfully then continue to create associations
                if(cohortResult.completedSuccessfully){
                    //2) start creating student cohort association
                    Task<IEnumerable<ActionResponseResult>> newStudentsAssociations;
                    if (obj.studentsToCreate != null && obj.studentsToCreate.Count() > 0)
                        newStudentsAssociations = CreateMultipleStudentCohortAssociations(cs, cohortResult.objectId, obj.studentsToCreate);
                    else
                        newStudentsAssociations = null;
                    //3) initial populate of the cohort custom entity
                    var cohortCustom = cs.CreateCohortCustom(cohortResult.objectId, JsonConvert.SerializeObject(obj.custom));

                    //contruct a list of tasks we're waiting for
                    var tasksToWaitFor = new List<Task>();
                    if (newStudentsAssociations != null) tasksToWaitFor.Add(newStudentsAssociations);
                    if (cohortCustom != null) tasksToWaitFor.Add(cohortCustom);

                    await Task.WhenAll(tasksToWaitFor);

                    if(newStudentsAssociations != null)
                        DetermineFailedToCreateFor(cohortResult, newStudentsAssociations);                  
                }
                return cohortResult;
            }
            catch (Exception e)
            {
                //handle
                throw;
            }            
        }

        /// <summary>
        /// AJAX to this method to update existing groups with students
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<Result> UpdateGroup(CohortActionObject obj)
        {
            try
            {
                var cs = new CohortService(Session["access_token"].ToString());
                //1) update cohort
                var cohortResult = await UpdateCohort(cs, obj.cohort); 
                //2) create student cohort association
                Task<IEnumerable<ActionResponseResult>> newStudentsAssociations;
                if (obj.studentsToCreate != null && obj.studentsToCreate.Count() > 0)
                    newStudentsAssociations = CreateMultipleStudentCohortAssociations(cs, cohortResult.objectId, obj.studentsToCreate);
                else
                    newStudentsAssociations = null;
                //3) update cohort custom entity
                var cohortCustom = cs.UpdateCohortCustom(obj.cohort.id, JsonConvert.SerializeObject(obj.custom));

                //4) remove students from cohort
                Task<IEnumerable<ActionResponseResult>> removeStudents;
                if (obj.studentsToDelete != null && obj.studentsToDelete.Count() > 0)
                {
                    //Get a list of the current studentCohortAssociations so that we have the ids to delete them from group
                    var currentStudentCohortAssociation = await cs.GetStudentCohortAssociationsByCohortId(obj.cohort.id);
                    //get the studentCohortAssociationId for students to delete
                    var associationToDelete = (from s in obj.studentsToDelete select (from csca in currentStudentCohortAssociation where csca.studentId == s select csca).Single());
                    //delete the studentCohortAssociation
                    removeStudents = DeleteMultipleStudentCohortAssociations(cs, associationToDelete);
                }
                else
                    removeStudents = null;

                //contruct a list of tasks we're waiting for
                var tasksToWaitFor = new List<Task>();
                if (newStudentsAssociations != null) tasksToWaitFor.Add(newStudentsAssociations);
                if (cohortCustom != null) tasksToWaitFor.Add(cohortCustom);
                if (removeStudents != null) tasksToWaitFor.Add(removeStudents);

                await Task.WhenAll(tasksToWaitFor);

                if (newStudentsAssociations != null) DetermineFailedToCreateFor(cohortResult, newStudentsAssociations);
                if (removeStudents != null) DetermineFailedToDeleteFor(cohortResult, removeStudents);

                //remove cohort from cache after an update
                HttpContext.Cache.Remove(obj.cohort.id);

                return cohortResult;
            }
            catch (Exception e)
            {
                //handle
                throw;
            }
            
        }

        /// <summary>
        /// AJAX to this method to delete a cohort by passing in a cohort id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> DeleteGroup(string id)
        {
            try
            {
                var cs = new CohortService(Session["access_token"].ToString());
                var associationToDelete = await cs.GetStudentCohortAssociationsByCohortId(id);
                //var associationToDelete = from csca in currentStudentCohortAssociation select csca.studentId;
                await DeleteMultipleStudentCohortAssociations(cs, associationToDelete); //remove associations for cohorts
                var result = await cs.DeleteById(id);
                return result.ToString();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Get all necessary data for initial page load
        /// </summary>
        /// <returns>data needed for page load</returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<ActionResult> Group()
        {
            var cs = new CohortService(Session["access_token"].ToString());
            var ss = new StudentService(Session["access_token"].ToString());

            var co = GetCohorts();
            var st = GetStudents();

            var dataElements = DataElementHelper.InitializeDataElements();

            await Task.WhenAll(co, st);

            var cohorts = Task.WhenAll(from c in co.Result select CohortHelper.GetCohortDisplayObject(cs, c));
            var students = Task.WhenAll(from s in st.Result select StudentHelper.GetStudentDisplayObject(ss, s));

            await Task.WhenAll(cohorts, students);
            await Task.WhenAll(dataElements);

            var data = new GroupingDisplayObject();
            data.cohorts = cohorts.Result;
            data.students = students.Result;
            data.dataElements = dataElements.Result;         

            data.filters = FilterHelper.InitializeFilters(); //contruct filter values to filter students in the app
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Sample()
        {

                //var displayObj = new List<CohortDisplayObject>();
                var cs = new CohortService(Session["access_token"].ToString());
                //var c = new StudentsController();
                //var students = await c.Get(Session["access_token"].ToString());
                var co = await GetCohorts();
                //var st = GetStudents();

                var displayObj = await Task.WhenAll(from c in co select CohortHelper.GetCohortDisplayObject(cs, c));
                //await Task.WhenAll(co, st);
                //var data = new Data { students = st, cohorts = co };
                //var cs = new CohortService(Session["access_token"].ToString());

                //var result = await Task.WhenAll(from c in co.Result select createMultipleAssociation(cs, c, st.Result));
                var filters = Helper.FilterHelper.InitializeFilters();
            
                return View(displayObj);            
        }

        public ActionResult LoginError()
        {
            return View("LoginError");
        }

        /// <summary>
        /// Create multiple StudentCohortAssociation object
        /// </summary>
        /// <param name="cs">the service to use for this action</param>
        /// <param name="associations">the StudentCohortAssociations to create</param>
        /// <returns>list of results</returns>
        public async Task<IEnumerable<ActionResponseResult>> CreateMultipleStudentCohortAssociations(CohortService cs, string cId, IEnumerable<string> sl)
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
        public async Task<ActionResponseResult> CreateOneStudentCohortAssociation(CohortService cs, string cId, string sId)
        {
            var a = new StudentCohortAssociation { cohortId = cId, studentId = sId, beginDate = DateTime.Now };
            var result = await cs.CreateStudentCohortAssociation(a);

            return new ActionResponseResult { data = sId, status = result };
        }

        /// <summary>
        /// Delete multiple StudentCohortAssociation objects
        /// </summary>
        /// <param name="cs">the service to use for this action</param>
        /// <param name="associations">the StudentCohortAssociations to delete</param>
        /// <returns>result of this action</returns>
        public async Task<IEnumerable<ActionResponseResult>> DeleteMultipleStudentCohortAssociations(CohortService cs, IEnumerable<StudentCohortAssociation> associations)
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
        public async Task<ActionResponseResult> DeleteOneStudentCohortAssociation(CohortService cs, StudentCohortAssociation association)
        {
            var result = await cs.DeleteStudentCohortAssociationById(association.id);
            return new ActionResponseResult { data = association.studentId, status = result };
        }

        /// <summary>
        /// Get all cohorts
        /// </summary>
        /// <returns>list of all cohorts the current user has access to</returns>
        public async Task<IEnumerable<Cohort>> GetCohorts(){
            var c = new CohortService(Session["access_token"].ToString());
            var cohorts = await c.GetAll();

            return cohorts;
        }

        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns>list of all students the current user has access to</returns>
        public async Task<IEnumerable<Student>> GetStudents()
        {
            var s = new StudentService(Session["access_token"].ToString());
            var students = await s.GetAll();

            return students;
        }

        /// <summary>
        /// Delete a cohort
        /// </summary>
        /// <param name="id">the id of the cohort to delete</param>
        public void DeleteCohort(string id)
        {
            try
            {
                var c = new CohortService(Session["access_token"].ToString());
                var result = c.DeleteById(id);
                //return result;
                Response.Redirect(MAIN);
            }
            catch
            {
                throw;
            }            
        }

        /// <summary>
        /// Create a single cohort
        /// </summary>
        /// <returns>result of the cohort creation</returns>
        public async Task<Result> CreateCohort(CohortService cs, Cohort cohort) //TODO: modify to accept a cohort argument
        {
            try
            {
                var userSession = (UserSession)Session[SLC_USER_SESSION];
                var result = new Result { completedSuccessfully = false }; //default to false, set to true later if it's successful

                //set temporary edorgid because it's null from the SLC API
                if (userSession != null && userSession.edOrgId != null && userSession.edOrgId != "")
                    cohort.educationOrgId = userSession.edOrgId;
                else
                    cohort.educationOrgId = "2012dh-836f96e7-0b25-11e2-985e-024775596ac8";               
                cohort.cohortType = SlcClient.Enum.CohortType.Other;

                var response = await cs.Create(cohort);

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    result.completedSuccessfully = true;
                    //another way of getting the Id: result.Headers.Location.AbsolutePath.Substring(result.Headers.Location.AbsolutePath.LastIndexOf("/") + 1)              
                    result.objectId = response.Headers.Location.Segments[5]; //getting the id from header location
                }

                return result;
            }
            catch
            {
                throw;
            }           
        }
        
        /// <summary>
        /// Update a single cohort
        /// </summary>
        /// <returns>result of the update</returns>
        public async Task<Result> UpdateCohort(CohortService cs, Cohort cohort)
        {
            try
            {
                var userSession = (UserSession)Session[SLC_USER_SESSION];
                var result = new Result { completedSuccessfully = false }; //default to false, set to true later if it's successful

                //user session has edOrgId == null but we need edOrgId to update a cohort
                if (userSession != null && userSession.edOrgId != null && userSession.edOrgId != "")
                    cohort.educationOrgId = userSession.edOrgId;
                else
                    cohort.educationOrgId = "2012dh-836f96e7-0b25-11e2-985e-024775596ac8";

                var response = await cs.Update(cohort);

                if (response == HttpStatusCode.OK)
                    result.completedSuccessfully = true;

                return result;
            }
            catch (Exception e)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Get the list of studentIds that the system failed to create associations for
        /// </summary>
        /// <param name="result">the result object to populate result to</param>
        /// <param name="associations">the response from the service for the attempted creation</param>
        private static void DetermineFailedToCreateFor(Result result, Task<IEnumerable<ActionResponseResult>> associations)
        {
            //determine if all the associations were created successfully
            result.completedSuccessfully = associations.Result.Any(a => a.status != HttpStatusCode.Created);
            if (!result.completedSuccessfully)
                result.partialCreateSuccess = associations.Result.Any(a => a.status == HttpStatusCode.Created);
            else
                result.partialCreateSuccess = false;

            result.failToCreateIds = from r in associations.Result where r.status != HttpStatusCode.Created select r.data;
        }

        /// <summary>
        /// Get the list of studentIds that the system failed to delete associations for
        /// </summary>
        /// <param name="result">the result object to populate result to</param>
        /// <param name="associations">the response from the service for the attempted deletion</param>
        private static void DetermineFailedToDeleteFor(Result result, Task<IEnumerable<ActionResponseResult>> associations)
        {
            //determine if all the associations were created successfully
            result.completedSuccessfully = associations.Result.Any(a => a.status != HttpStatusCode.NoContent);
            if (!result.completedSuccessfully)
                result.partialDeleteSuccess = associations.Result.Any(a => a.status == HttpStatusCode.NoContent);
            else
                result.partialDeleteSuccess = false;

            result.failToCreateIds = from r in associations.Result where r.status != HttpStatusCode.NoContent select r.data;
        }

        /// <summary>
        /// SLC OAuth
        /// </summary>
        /// <param name="redirectUrl">url to redirect to after successful authentication</param>
        [NonAction]
        private void GetToken(string redirectUrl)
        {
            // We get a code back from the first leg of OAuth process.  If we don't have one, let's get it.
            if (Request.QueryString["code"] == null)
            {
                // Here the user will log into the SLC.
                string authorizeUrl = string.Format(SLC_SANDBOX_LOGIN, SLC_CLIENT_ID, SLC_REDIRECT_URL);
                Response.Redirect(authorizeUrl);
            }
            else
            {
                // Now we have a code, we can run the second leg of OAuth process.
                string code = Request.QueryString["code"];

                // Set the authorization URL
                string sessionUrl = string.Format(SLC_OAUTH_URL, SLC_CLIENT_ID, SLC_SHARED_SECRET, SLC_REDIRECT_URL, code);

                var client = new HttpClient();
                var response = client.GetAsync(sessionUrl).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string access_token = JObject.Parse(response.Content.ReadAsStringAsync().Result)["access_token"].ToString();
                    // If we have a valid token, it'll be 38 chars long.  Let's add it to session if so.
                    if (access_token.Length == 38)
                    {
                        Session.Add("access_token", access_token);

                        //Get the current user session info
                        var ss = new SessionService(access_token);
                        var userSession = ss.Get().Result;
                        Session.Add(SLC_USER_SESSION, userSession);

                        // Redirect to app main page.
                        Response.Redirect(redirectUrl);
                    }
                }
                else
                {
                    //error logging into SLC
                    Response.Redirect("Home/LoginError");
                }
            }
        }
    }
}
