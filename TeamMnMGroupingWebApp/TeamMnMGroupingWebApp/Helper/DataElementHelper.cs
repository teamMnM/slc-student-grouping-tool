using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TeamMnMGroupingWebApp.Models;

namespace TeamMnMGroupingWebApp.Helper
{
    /// <summary>
    /// Contruct data elements to display
    /// </summary>
    public class DataElementHelper
    {
        public static async Task<IEnumerable<DataElement>> InitializeDataElements()
        {
            try
            {
                using (StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + "\\Data\\DataElements.txt"))
                {
                    string s = await sr.ReadToEndAsync();
                    var list = JsonConvert.DeserializeObject<IEnumerable<DataElement>>(s);
                    return list;
                }
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}