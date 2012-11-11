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

namespace TeamMnMGroupingWebApp.Controllers
{
    [AsyncTimeout(5000)]
    [HandleError(ExceptionType = typeof(TimeoutException), View = "Timeout")]
    [HandleError(ExceptionType = typeof(NullReferenceException), View = "Error")]
    [HandleError(ExceptionType = typeof(HttpRequestException), View = "PermissionError")]
    public class HomeController : BaseController
    {
        const string MAIN = "/Home/Sample";

        public void Index()
        {
            Response.Redirect("/Home/Index2");
            //if (Session["access_token"] == null)
            //{
            //    GetToken(MAIN);               
            //}
            //else
            //{
            //    // We have an access token in session, let's redirect to app main page.
            //    Response.Redirect(MAIN);
            //}
        }

        public ActionResult Index2()
        {
            return View("Index");
        }

        public void AbandonSession()
        {
            Session.Abandon();
            Response.Redirect("/Home/Index");
        }

        /// <summary>
        /// AJAX to this method to create brand new groups with students
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<string> CreateGroup(CohortActionObject obj)
        {
            try
            {
                var cs = new CohortService(Session["access_token"].ToString());
                var cohortId = await CreateCohort(cs, obj.cohort); //1) create the cohort first and retrieve the Id of the new cohort
                var studentsAssociations = CreateMultipleAssociation(cs, cohortId, obj.studentsToCreate); //2) start creating student cohort association
                var cohortCustom = cs.CreateCohortCustom(cohortId, JsonConvert.SerializeObject(obj.custom)); //3) initial populate of the cohort custom entity

                await Task.WhenAll(studentsAssociations, cohortCustom);

                return cohortId;
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
        public async Task<string> UpdateGroup(CohortActionObject obj)
        {
            try
            {
                var cs = new CohortService(Session["access_token"].ToString());
                var cohort = UpdateCohort(cs, obj.cohort); //1) update cohort
                var newStudentsAssociations = CreateMultipleAssociation(cs, obj.cohort.id, obj.studentsToCreate); //2) create student cohort association
                var cohortCustom = cs.UpdateCohortCustom(obj.cohort.id, JsonConvert.SerializeObject(obj.custom)); //3) update cohort custom entity

                //Get a list of the current studentCohortAssociations so that we have the ids to delete them from group
                var currentStudentCohortAssociation = await cs.GetStudentCohortAssociationsByCohortId(obj.cohort.id);
                //get the studentCohortAssociationId for students to delete
                var associationToDelete = (from s in obj.studentsToDelete select (from csca in currentStudentCohortAssociation where csca.studentId == s.id select csca.id).Single());
                //delete the studentCohortAssociation
                var removeStudents = DeleteMultipleAssociation(cs, associationToDelete); 

                await Task.WhenAll(newStudentsAssociations, cohortCustom, removeStudents);

                return "Success";
            }
            catch (Exception e)
            {
                //handle
                throw;
            }
            
        }

        public async Task<ActionResult> Group()
        {
            var cs = new CohortService(Session["access_token"].ToString());
            var ss = new StudentService(Session["access_token"].ToString());

            var co = GetCohorts();
            var st = GetStudents();

            await Task.WhenAll(co, st);

            var cohorts = Task.WhenAll(from c in co.Result select CohortHelper.GetCohortDisplayObject(cs, c));
            var students = Task.WhenAll(from s in st.Result select StudentHelper.GetStudentDisplayObject(ss, s));

            await Task.WhenAll(cohorts, students);

            var data = new GroupingDisplayObject();
            data.cohorts = cohorts.Result;
            data.students = students.Result;
            data.filters = FilterHelper.InitializeFilters();

            return View(data);
        }

        //public async Task<string[]> CreateCustom(CohortCustom obj)
        //{
        //    var cs = new CohortService(Session["access_token"].ToString());
        //    var result = await cs.CreateCohortCustom
        //}

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

        public async Task<ActionResult> Student()
        {

            //var displayObj = new List<CohortDisplayObject>();
            var cs = new StudentService(Session["access_token"].ToString());
            //var c = new StudentsController();
            //var students = await c.Get(Session["access_token"].ToString());
            var st = await GetStudents();
            //var st = GetStudents();

            var displayObj = await Task.WhenAll(from s in st select StudentHelper.GetStudentDisplayObject(cs, s));
            //await Task.WhenAll(co, st);
            //var data = new Data { students = st, cohorts = co };
            //var cs = new CohortService(Session["access_token"].ToString());

            //var result = await Task.WhenAll(from c in co.Result select createMultipleAssociation(cs, c, st.Result));

            return View(displayObj);
        }

        public ActionResult LoginError()
        {
            return View("LoginError");
        }

        public async Task<string[]> CreateMultipleAssociation(CohortService cs, string cId, IEnumerable<Student> sl)
        {
            var result = await Task.WhenAll(from s in sl select CreateOneAssociation(cs, cId, s));
            return result;
        }


        public async Task<string> CreateOneAssociation(CohortService cs, string cId, Student s)
        {
            var a = new StudentCohortAssociation { cohortId = cId, studentId = s.id, beginDate = DateTime.Now };
            var result = await cs.CreateStudentCohortAssociation(a);

            return result.ToString();
        }

        public async Task<string[]> DeleteMultipleAssociation(CohortService cs, IEnumerable<string> associations)
        {
            var result = await Task.WhenAll(from a in associations select DeleteOneAssociation(cs, a));
            return result;
        }

        public async Task<string> DeleteOneAssociation(CohortService cs, string associationId)
        {
            var result = await cs.DeleteStudentCohortAssociationById(associationId);

            return result.ToString();
        }

        public async Task<IEnumerable<Cohort>> GetCohorts(){
            var c = new CohortService(Session["access_token"].ToString());
            var cohorts = await c.GetAll();

            return cohorts;
        }

        public async Task<IEnumerable<Student>> GetStudents()
        {
            var s = new StudentService(Session["access_token"].ToString());
            var students = await s.GetAll();

            return students;
        }

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
            //throw new Exception("break here");
        }

        /// <summary>
        /// Create a single cohort
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateCohort(CohortService cs, Cohort cohort) //TODO: modify to accept a cohort argument
        {
            try
            {
                //var cs = new CohortService(Session["access_token"].ToString());
                //var cohort = new Cohort();
                //var random = new Random();
                //cohort.educationOrgId = "2012dh-836f96e7-0b25-11e2-985e-024775596ac8";
                //cohort.cohortIdentifier = "ACC-TEST-COH-" + random.Next(10);
                //cohort.cohortType = SlcClient.Enum.CohortType.ExtracurricularActivity;
                var result = await cs.Create(cohort);
                //result.Headers.Location.AbsolutePath.Substring(result.Headers.Location.AbsolutePath.LastIndexOf("/") + 1)
                
                return result.Headers.Location.Segments[5]; //getting the id from header location
            }
            catch
            {
                throw;
            }           
        }

        /// <summary>
        /// Create multiple new cohorts
        /// </summary>
        /// <param name="cohorts">new cohorts to create</param>
        /// <returns></returns>
        //public async Task<string[]> CreateCohorts(IEnumerable<Cohort> cohorts)
        //{
        //    try
        //    {
        //        var result = await Task.WhenAll(from c in cohorts select CreateCohort()); //TODO: add cohort as parameter
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Update a single cohort
        /// </summary>
        /// <returns></returns>
        public async Task<string> UpdateCohort(CohortService cs, Cohort cohort)
        {
            //var c = new CohortService(Session["access_token"].ToString());
            //var cohort = new Cohort();
            //var random = new Random();
            //cohort.id = "2012gd-cae52ab2-26f7-11e2-8acf-02786562673b";
            //cohort.educationOrgId = "2012dh-836f96e7-0b25-11e2-985e-024775596ac8";
            //cohort.cohortIdentifier = "ACC-" + DateTime.Now.Ticks.ToString().Substring(0,8);
            //cohort.cohortType = SlcClient.Enum.CohortType.ExtracurricularActivity;
            var result = await cs.Update(cohort);
            return result.ToString();
        }

        /// <summary>
        /// Update multiple new cohorts
        /// </summary>
        /// <param name="cohorts">new cohorts to create</param>
        /// <returns></returns>
        //public async Task<string[]> UpdateCohorts(IEnumerable<Cohort> cohorts)
        //{
        //    var result = await Task.WhenAll(from c in cohorts select UpdateCohort()); //TODO: add cohort as parameter
        //    return result;
        //}

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

    public class Data
    {
        public Task<IEnumerable<Student>> students { get; set; }
        public Task<IEnumerable<Cohort>> cohorts { get; set; }
    }
}
