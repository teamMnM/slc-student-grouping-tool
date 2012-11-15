using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace SlcClient
{
    /// <summary>
    /// HttpClient to interact with the SLC API
    /// </summary>
    class SlcHttpClient
    {
        private HttpClient _httpClient;
        private string _token;
        private const string SLC_API_SANDBOX_URL = "SlcApiSandboxUrl";

        public SlcHttpClient(string token)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings[SLC_API_SANDBOX_URL]);
            _token = token;
        }

        public async Task<HttpResponseMessage> GetDataString(string endPoint)
        {
            try
            {               
                var response = await _httpClient.GetAsync(endPoint);
                //response.EnsureSuccessStatusCode(); //throw exception if status is non-successful

                //var responseBodyAsText = await response.Content.ReadAsStringAsync();
                return response;
            }
            catch (Exception ex)
            {
                //log exception here
                throw;
            }
        }

        public async Task<HttpResponseMessage> PostData(string endPoint, object param)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endPoint, param);
                //response.EnsureSuccessStatusCode();

                //var strResponse = await response.Content.ReadAsStringAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> PutData(string endPoint, object param)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(endPoint, param);
                //response.EnsureSuccessStatusCode();

                //var strResponse = await response.Content.ReadAsStringAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> DeleteData(string endPoint, string id)
        {
            try
            {
                var strEndPoint = string.Format("{0}{1}", endPoint, id);
                var response = await _httpClient.DeleteAsync(strEndPoint);
                //response.EnsureSuccessStatusCode();

                //var strResponse = await response.Content.ReadAsStringAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
