using Newtonsoft.Json;
using SlcClient.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SlcClient.Services
{
    public class SessionService
    {
        private SlcHttpClient _client;
        private const string SLC_API_SANDBOX_SESSION_URL = "SlcApiSandboxSessionUrl";

        public SessionService(string token)
        {
            _client = new SlcHttpClient(token, SLC_API_SANDBOX_SESSION_URL);
        }

        public async Task<UserSession> Get()
        {
            try
            {
                var response = await _client.GetDataString("");
                var content = await response.Content.ReadAsStringAsync();
                var session = JsonConvert.DeserializeObject<UserSession>(content);
                return session; 
            }
            catch (Exception e)
            {
                //log here
                throw;
            }    
        }
    }
}
