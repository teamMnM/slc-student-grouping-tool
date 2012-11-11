using Newtonsoft.Json;
using SlcClient.Entities;
using SlcClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TeamMnMGroupingWebApp.Models;

namespace TeamMnMGroupingWebApp.Helper
{
    public class CohortHelper
    {
        public static async Task<CohortDisplayObject> GetCohortDisplayObject(CohortService cs, Cohort cohort)
        {
            var students = GetStudentsByCohortId(cs, cohort.id);
            var custom = GetCohortCustomByCohortId(cs, cohort.id);

            await Task.WhenAll(students, custom);

            var displayObject = new CohortDisplayObject();
            displayObject.cohort = cohort;
            displayObject.students = from s in students.Result select s.id;
            displayObject.custom = JsonConvert.DeserializeObject<CohortCustom>(custom.Result); ;

            return displayObject;
        }

        private static async Task<IEnumerable<Student>> GetStudentsByCohortId(CohortService cs, string cohortId)
        {
            var result = await cs.GetStudentsByCohortId(cohortId);
            return result;
        }

        private static async Task<string> GetCohortCustomByCohortId(CohortService cs, string cohortId)
        {
            try
            {
                var result = await cs.GetCustomById(cohortId);
                return result;
            }
            catch (Exception ex)
            {
                //when there isn't a custom yet, SLC throw a 404
                return "";
            }
            
        }
    }
}