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
    public class GlobalHelper
    {
        /// <summary>
        /// Get data elements from file
        /// </summary>
        /// <returns>List of data elements in file in ~/Data/DataElements.txt</returns>
        public static async Task<IEnumerable<DataElement>> InitializeDataElements()
        {
            try
            {
                    const string path = "\\Data\\DataElements.txt";
                    string s = await GetJsonFromFile(path);
                    var list = JsonConvert.DeserializeObject<IEnumerable<DataElement>>(s);
                    return list;

            }
            catch(Exception e)
            {
                ExceptionHelper.LogCaughtException(e);
                return new List<DataElement>();
            }
        }

        /// <summary>
        /// Get list of color from file
        /// </summary>
        /// <returns>List of colors in file in ~/Data/Colors.txt</returns>
        public static async Task<IEnumerable<Color>> InitializeColors()
        {
            try
            {
                const string path = "\\Data\\Colors.txt";
                string s = await GetJsonFromFile(path);
                var list = JsonConvert.DeserializeObject<IEnumerable<Color>>(s);
                return list;

            }
            catch (Exception e)
            {
                ExceptionHelper.LogCaughtException(e);
                return new List<Color>();
            }
        }

        public static async Task<string> GetJsonFromFile(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + path))
                {
                    string s = await sr.ReadToEndAsync();
                    return s;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}