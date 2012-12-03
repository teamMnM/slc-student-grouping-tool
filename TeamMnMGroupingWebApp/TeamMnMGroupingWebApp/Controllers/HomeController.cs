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

namespace TeamMnMGroupingWebApp.Controllers
{
    [AsyncTimeout(5000)]
    [HandleError(ExceptionType = typeof(TimeoutException), View = "Timeout")]
    [HandleError(ExceptionType = typeof(NullReferenceException), View = "Error")]
    [HandleError(ExceptionType = typeof(HttpRequestException), View = "PermissionError")]
    public class HomeController : BaseController
    {
        const string MAIN = "/Home/GroupSelection";
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

        /// <summary>
        /// Log user out of the current session
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Logout()
        {
            var token = Session["access_token"];
            if (token != null)
            {
                try
                {
                    var ss = new SessionService(token.ToString());
                    var result = await ss.Logout();
                    Session.Clear();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    //logout fail
                    Session.Clear();
                    return Json(new LogOutResult { logout = false, msg = e.Message }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                //user is already logged out
                return Json(new LogOutResult { logout = true, msg = "There was no access token" }, JsonRequestBehavior.AllowGet);
            }            
        }

        //[OutputCache(Duration = 1200, VaryByParam = "none")]
        public ActionResult MultipleGroupsEdit()
        {
            return View("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GroupSelection()
        {
            return View("GroupSelection");
        }

        /// <summary>
        /// AJAX to this method to create a brand new group with students
        /// </summary>
        /// <param name="obj">the cohort to delete</param>
        /// <returns>result of the delete</returns>
        public async Task<ActionResult> CreateGroup(CohortActionObject obj)
        {
            try
            {
                var cohortResult = await ProcessOneCohortCreate(obj);
                return Json(cohortResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //handle
                throw;
            }
        }

        /// <summary>
        /// AJAX to this method to create brand new groups with students
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Result>> CreateMultipleGroups(IEnumerable<CohortActionObject> objs)
        {
            try
            {
                var result = await Task.WhenAll(from obj in objs select ProcessOneCohortCreate(obj));
                return result;
            }
            catch (Exception e)
            {
                //handle
                throw;
            }
        }

        /// <summary>
        /// AJAX to this method to update an existing group
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<ActionResult> UpdateGroup(CohortActionObject obj)
        {
            try
            {
                var cohortResult = await ProcessOneCohortUpdate(obj);
                return Json(cohortResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //handle
                throw;
            }
        }

        /// <summary>
        /// AJAX to this method to update multiple groups
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Result>> UpdateMultipleGroups(IEnumerable<CohortActionObject> objs)
        {
            try
            {
                var result = await Task.WhenAll(from obj in objs select ProcessOneCohortUpdate(obj));
                return result;
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
        /// <param name="id">id of the cohort to delete</param>
        /// <returns>result</returns>
        public async Task<ActionResult> DeleteGroup(string id)
        {
            try
            {
                var cohortResult = await ProcessOneCohortDelete(id);
                return Json(cohortResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// AJAX to this method to delete multiple groups
        /// </summary>
        /// <param name="ids">list of cohort ids to delete</param>
        /// <returns>result</returns>
        public async Task<ActionResult> DeleteMultipleGroups(IEnumerable<string> ids)
        {
            try
            {
                var result = await Task.WhenAll(from id in ids select ProcessOneCohortDelete(id));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //handle
                throw;
            }
        }

        /// <summary>
        /// Update one cohort
        /// </summary>
        /// <param name="obj">data object to update cohort</param>
        /// <returns>result of the action</returns>
        public async Task<Result> ProcessOneCohortUpdate(CohortActionObject obj)
        {
            try
            {
                var accessToken = Session["access_token"];
                if (accessToken != null)
                {
                    var cs = new CohortService(Session["access_token"].ToString());
                    //1) update cohort
                    var cohortResult = await UpdateCohort(cs, obj.cohort);
                    //2) create student cohort association                    
                    var newStudentsAssociations = GetNewStudentCohortAssociations(obj, cs);
                    //3) update cohort custom entity
                    var custom = obj.custom;
                    if (custom == null) custom = new CohortCustom { lastModifiedDate = DateTime.UtcNow };
                    else custom.lastModifiedDate = DateTime.UtcNow;
                    var cohortCustom = cs.UpdateCohortCustom(obj.cohort.id, JsonConvert.SerializeObject(custom));

                    //4) remove students from cohort
                    Task<IEnumerable<ActionResponseResult>> removeStudents;
                    if (obj.studentsToDelete != null && obj.studentsToDelete.Count() > 0)
                    {
                        //Get a list of the current studentCohortAssociations so that we have the ids to delete them from group
                        var currentStudentCohortAssociation = await cs.GetStudentCohortAssociationsByCohortId(obj.cohort.id);
                        //get the studentCohortAssociationId for students to delete
                        var associationToDelete = (from s in obj.studentsToDelete
                                                   select (currentStudentCohortAssociation.FirstOrDefault(csca => csca.studentId == s)));
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

                    if (newStudentsAssociations != null) DetermineFailedToCreateFor(cohortResult, newStudentsAssociations.Result);
                    if (removeStudents != null) DetermineFailedToDeleteFor(cohortResult, removeStudents.Result);

                    //determine whether custom was created successfully
                    ProcessCustomResult(cohortResult, cohortCustom, HttpStatusCode.NoContent);

                    //remove cohort from cache after an update
                    HttpContext.Cache.Remove(obj.cohort.id);

                    return cohortResult;
                }
                else
                {
                    //session expired
                    return GetSessionExpiredResult(obj.cohort.id);
                }
            }
            catch (Exception e)
            {
                return GetExceptionResult(obj.cohort.id, e);
            }

        }

        /// <summary>
        /// Create new student cohort associations
        /// </summary>
        /// <param name="obj">cohort to create</param>
        /// <param name="cs">cohort service</param>
        /// <returns>result of this action</returns>
        private Task<IEnumerable<ActionResponseResult>> GetNewStudentCohortAssociations(CohortActionObject obj, CohortService cs)
        {
            Task<IEnumerable<ActionResponseResult>> newStudentsAssociations;
            if (obj.studentsToCreate != null && obj.studentsToCreate.Count() > 0)
                newStudentsAssociations = CreateMultipleStudentCohortAssociations(cs, obj.cohort.id, obj.studentsToCreate);
            else
                newStudentsAssociations = null;
            return newStudentsAssociations;
        }

        /// <summary>
        /// Create one cohort
        /// </summary>
        /// <param name="obj">data object to create cohort</param>
        /// <returns>result of the action</returns>
        public async Task<Result> ProcessOneCohortCreate(CohortActionObject obj)
        {
            try
            {
                var accessToken = Session["access_token"];
                if (accessToken != null)
                {
                    var cs = new CohortService(Session["access_token"].ToString());
                    //1) create the cohort first
                    var cohortResult = await CreateCohort(cs, obj.cohort);

                    //if cohort was created successfully then continue to create associations
                    if (cohortResult.completedSuccessfully)
                    {
                        obj.cohort.id = cohortResult.objectId;
                        //2) start creating student cohort association
                        var newStudentsAssociations = GetNewStudentCohortAssociations(obj, cs);
                        //3) initial populate of the cohort custom entity
                        var custom = obj.custom;
                        if (custom == null) custom = new CohortCustom { lastModifiedDate = DateTime.UtcNow };
                        else custom.lastModifiedDate = DateTime.UtcNow;
                        var cohortCustom = cs.CreateCohortCustom(cohortResult.objectId, JsonConvert.SerializeObject(custom));

                        //contruct a list of tasks we're waiting for
                        var tasksToWaitFor = new List<Task>();
                        if (newStudentsAssociations != null) tasksToWaitFor.Add(newStudentsAssociations);
                        if (cohortCustom != null) tasksToWaitFor.Add(cohortCustom);

                        await Task.WhenAll(tasksToWaitFor);

                        if (newStudentsAssociations != null)
                            DetermineFailedToCreateFor(cohortResult, newStudentsAssociations.Result);

                        //determine whether custom was created successfully
                        ProcessCustomResult(cohortResult, cohortCustom, HttpStatusCode.Created);

                    }

                    return cohortResult;
                }
                else
                {
                    //section expired
                    return GetSessionExpiredResult(obj.cohort.cohortIdentifier);
                }

            }
            catch (Exception e)
            {
                return GetExceptionResult(obj.cohort.cohortIdentifier, e);
            }
        }

        private static void ProcessCustomResult(Result cohortResult, Task<HttpResponseMessage> cohortCustom, HttpStatusCode successStatus)
        {
            var customResult = GetActionResponseResult(cohortResult.objectId, cohortCustom.Result, successStatus);
            if (cohortCustom.Result.StatusCode != successStatus)
                cohortResult.completedSuccessfully = false;

            cohortResult.customActionResult = customResult;
        }

        /// <summary>
        /// Delete one cohort
        /// </summary>
        /// <param name="obj">data object to deletes cohort</param>
        /// <returns>result of the action</returns>
        public async Task<Result> ProcessOneCohortDelete(string id)
        {
            try
            {
                var accessToken = Session["access_token"];
                if (accessToken != null)
                {
                    var cs = new CohortService(Session["access_token"].ToString());
                    var associationToDelete = await cs.GetStudentCohortAssociationsByCohortId(id);
                    ////var associationToDelete = from csca in currentStudentCohortAssociation select csca.studentId;               
                    var cohortResult = await DeleteCohort(cs, id);

                    if (associationToDelete != null && associationToDelete.Count() > 0)
                    {
                        IEnumerable<ActionResponseResult> removeStudents = await
                            DeleteMultipleStudentCohortAssociations(cs, associationToDelete); //remove associations for cohorts
                        if (removeStudents != null) DetermineFailedToDeleteFor(cohortResult, removeStudents);
                    }

                    //remove cohort from cache after an update
                    HttpContext.Cache.Remove(id);

                    return cohortResult;
                }
                else
                {
                    //session has expired
                    return GetSessionExpiredResult(id);
                }
            }
            catch (Exception e)
            {
                return GetExceptionResult(id, e);
            }
        }

        /// <summary>
        /// AJAX to this method for a master save of all groups
        /// </summary>
        /// <param name="list">list of CohortActionObject to save</param>
        /// <returns>list of result of this action</returns>
        public async Task<ActionResult> SaveAll(IEnumerable<CohortActionObject> list)
        {
            try
            {
                var cohortsToUpdate = from cao in list where cao.cohort.id != null select cao;
                var cohortsToCreate = from cao in list where cao.cohort.id == null select cao;

                var updateCohorts = UpdateMultipleGroups(cohortsToUpdate);
                var createCohorts = CreateMultipleGroups(cohortsToCreate);

                var allTasks = await Task.WhenAll(updateCohorts, createCohorts);

                var resultList = new List<Result>();
                resultList.AddRange(updateCohorts.Result);
                resultList.AddRange(createCohorts.Result);

                return Json(resultList, JsonRequestBehavior.AllowGet);
            }
            catch
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
            var accessToken = Session["access_token"];
            if (accessToken != null)
            {
                var cs = new CohortService(accessToken.ToString());
                var ss = new StudentService(accessToken.ToString());
                var ses = new SectionService(accessToken.ToString());

                var co = GetCohorts();
                var st = GetStudents();
                var se = GetSections();

                //Get the available data elements and colors
                var dataElements = GlobalHelper.InitializeDataElements();
                var colors = GlobalHelper.InitializeColors();
                var filters = FilterHelper.InitializeFilters(); //contruct filter values to filter students in the app

                await Task.WhenAll(co, st, se);

                var cohorts = Task.WhenAll(from c in co.Result select CohortHelper.GetCohortDisplayObject(cs, c));
                var students = Task.WhenAll(from s in st.Result select StudentHelper.GetStudentDisplayObject(ss, s));
                var sections = Task.WhenAll(from s in se.Result select SectionHelper.GetSectionDisplayObject(ses, s));

                await Task.WhenAll(cohorts, students, se);
                await Task.WhenAll(dataElements, colors);

                //Construct a master object to for display purpose
                var data = new GroupingDisplayObject();
                data.cohorts = cohorts.Result;
                data.students = students.Result;
                data.sections = sections.Result;
                data.dataElements = dataElements.Result;
                data.colors = colors.Result;
                data.filters = filters;

                return Json(data, JsonRequestBehavior.AllowGet);
            }

            //session has expired, refresh page
            return View("Index");

        }

        /// <summary>
        /// Return the LoginError view
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginError()
        {
            return View();
        }

        /// <summary>
        /// Return the Error view
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
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

            return GetActionResponseResult(sId, result, HttpStatusCode.Created);
            //return new ActionResponseResult { data = sId, status = result.StatusCode, message = result.Content.ReadAsStringAsync().Result };
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
            var response = await cs.DeleteStudentCohortAssociationById(association.id);
            return GetActionResponseResult(association.studentId, response, HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Get all cohorts from SLI for the current user session
        /// </summary>
        /// <returns>list of all cohorts the current user has access to</returns>
        public async Task<IEnumerable<Cohort>> GetCohorts()
        {
            var c = new CohortService(Session["access_token"].ToString());
            var cohorts = await c.GetAll();

            return cohorts;
        }

        /// <summary>
        /// Get all students from SLI for the current user session
        /// </summary>
        /// <returns>list of all students the current user has access to</returns>
        public async Task<IEnumerable<Student>> GetStudents()
        {
            var s = new StudentService(Session["access_token"].ToString());
            var students = await s.GetAll();

            return students;
        }

        /// <summary>
        /// Get all sections from SLI for the current user session
        /// </summary>
        /// <returns>List of all sections the current user has access to</returns>
        public async Task<IEnumerable<Section>> GetSections()
        {
            var c = new SectionService(Session["access_token"].ToString());
            var list = await c.GetAll();

            return list;
        }

        /// <summary>
        /// Delete a cohort
        /// </summary>
        /// <param name="id">the id of the cohort to delete</param>
        public async Task<Result> DeleteCohort(CohortService cs, string id)
        {
            try
            {
                var result = new Result { completedSuccessfully = false }; //default to false, set to true later if it's successful
                var response = await cs.DeleteById(id);

                if (response.StatusCode == HttpStatusCode.NoContent)
                    result.completedSuccessfully = true;

                return result;
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

                //set temporary edorgid because it's null from the SLC API
                if (userSession != null && userSession.edOrgId != null && userSession.edOrgId != "")
                    cohort.educationOrgId = userSession.edOrgId;
                else
                    cohort.educationOrgId = CURRENT_ED_ORG_ID;
                cohort.cohortType = SlcClient.Enum.CohortType.Other;

                var response = await cs.Create(cohort);

                var result = new Result
                {
                    completedSuccessfully = response.StatusCode == HttpStatusCode.Created,
                    objectActionResult = GetActionResponseResult("", response, HttpStatusCode.Created)
                };

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    //another way of getting the Id: result.Headers.Location.AbsolutePath.Substring(result.Headers.Location.AbsolutePath.LastIndexOf("/") + 1)              
                    result.objectId = response.Headers.Location.Segments[5]; //getting the id from header location
                    result.objectActionResult.data = result.objectId;
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

                //user session has edOrgId == null but we need edOrgId to update a cohort
                if (userSession != null && userSession.edOrgId != null && userSession.edOrgId != "")
                    cohort.educationOrgId = userSession.edOrgId;
                else
                    //cohort.educationOrgId = "2012dh-836f96e7-0b25-11e2-985e-024775596ac8"; // daom 
                    cohort.educationOrgId = CURRENT_ED_ORG_ID;

                var response = await cs.Update(cohort);

                var result = new Result
                {
                    objectId = cohort.id,
                    completedSuccessfully = response.StatusCode == HttpStatusCode.NoContent,
                    objectActionResult = GetActionResponseResult(cohort.id, response, HttpStatusCode.NoContent)
                };

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
        private static void DetermineFailedToCreateFor(Result result, IEnumerable<ActionResponseResult> associations)
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
        private static void DetermineFailedToDeleteFor(Result result, IEnumerable<ActionResponseResult> associations)
        {
            //determine if all the associations were created successfully
            var assocationSuccess = associations.All(a => a.status == HttpStatusCode.NoContent);
            if (result.completedSuccessfully) //if overal success is true and set to to false if this action is false, else overal success is still false
                result.completedSuccessfully = assocationSuccess;

            if (!result.completedSuccessfully)
                result.partialDeleteSuccess = associations.Any(a => a.status == HttpStatusCode.NoContent);
            else
                result.partialDeleteSuccess = false;

            result.failToCreateAssocations = from r in associations where r.status != HttpStatusCode.NoContent select r;
        }

        /// <summary>
        /// SLC OAuth
        /// </summary>
        /// <param name="redirectUrl">url to redirect to after successful authentication</param>
        [NonAction]
        private void GetToken(string redirectUrl)
        {
            try
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
            catch (Exception e)
            {
                ExceptionHelper.LogCaughtException(e);
                Response.Redirect("Home/LoginError");
            }
        }

        /// <summary>
        /// Get a Result object with an expired status
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Result GetSessionExpiredResult(string id = "")
        {
            return new Result
            {
                completedSuccessfully = false,
                objectId = id,
                objectActionResult =
                    new ActionResponseResult
                    {
                        status = HttpStatusCode.ProxyAuthenticationRequired,
                        message = "Session expired",
                        isSuccess = false
                    }
            };
        }

        /// <summary>
        /// Get a Result object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private static Result GetExceptionResult(string id, Exception e)
        {
            ExceptionHelper.LogCaughtException(e);
            return new Result
            {
                completedSuccessfully = false,
                objectId = id,
                objectActionResult =
                    new ActionResponseResult
                    {
                        status = HttpStatusCode.InternalServerError,
                        message = "Message: " + e.Message + " Inner Exception: " + e.InnerException == null ? "" : e.InnerException.Message,
                        isSuccess = false
                    }
            };
        }

        /// <summary>
        /// Create a new ActionResponseResult object base on the HttpResponseMessage parameter
        /// </summary>
        /// <param name="objId">The related object id</param>
        /// <param name="m"></param>
        /// <returns></returns>
        private static ActionResponseResult GetActionResponseResult(string objId, HttpResponseMessage m, HttpStatusCode successStatus)
        {
            return new ActionResponseResult
            {
                data = objId,
                status = m.StatusCode,
                message = m.Content.ReadAsStringAsync().Result,
                isSuccess = m.StatusCode == successStatus
            };
        }
    }
}
