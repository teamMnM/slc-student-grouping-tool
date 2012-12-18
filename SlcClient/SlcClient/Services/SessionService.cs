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
        private const string SLC_API_SANDBOX_SESSION_CHECK_URL = "check/";
        private const string SLC_API_SANDBOX_SESSION_LOGOUT_URL = "logout/";
        private const string SLC_API_SANDBOX_SESSION_DEBUG_URL = "debug/";

        public SessionService(string token)
        {
            _client = new SlcHttpClient(token, SLC_API_SANDBOX_SESSION_URL);
        }

        public async Task<UserSession> Get()
        {
            try
            {
                var response = await _client.GetDataString(SLC_API_SANDBOX_SESSION_CHECK_URL);
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

        public async Task<LogOutResult> Logout()
        {
            try
            {
                var response = await _client.GetDataString(SLC_API_SANDBOX_SESSION_LOGOUT_URL);
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<LogOutResult>(content);
                return result;
            }
            catch (Exception e)
            {
                //log here
                throw;
            }
        }

        public async Task<DebugResult> Debug()
        {
            try
            {
                var response = await _client.GetDataString(SLC_API_SANDBOX_SESSION_DEBUG_URL);
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<DebugResult>(content);
                return result;
            }
            catch (Exception e)
            {
                //log here
                throw;
            }
        }
    }
}
