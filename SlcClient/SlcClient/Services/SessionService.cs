using Newtonsoft.Json;
using SlcClient.Entities;
using SlcClient.Entities.Authentication;
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
        private const string SLC_API_SANDBOX_SESSION_CHECK_URL = "check/";
        private const string SLC_API_SANDBOX_SESSION_LOGOUT_URL = "logout/";
        private const string SLC_API_SANDBOX_SESSION_DEBUG_URL = "debug/";

        public SessionService(string token)
        {
            _client = new SlcHttpClient(token, Properties.Settings.Default.SlcApiSandboxSessionUrl);
        }

        /// <summary>
        /// Get the current user session info
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Call to SLC to log the current user out
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get the current user debug session
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get links for current user
        /// </summary>
        /// <returns></returns>
        public async Task<Home> Home()
        {
            try
            {
                //create new client because Home uses /rest/v1 isntead of /rest/system/check
                var client = new SlcHttpClient(_client.token, Properties.Settings.Default.SlcApiSandboxUrl);
                var response = await client.GetDataString(Constants.Session.HOME);
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Home>(content);
                return result;
            }
            catch (Exception e)
            {
                //log here
                throw;
            }
        }

        /// <summary>
        /// Get current user Id
        /// </summary>
        /// <returns>The Id of the current user</returns>
        public async Task<string> GetCurrentUserId()
        {
            try
            {
                var result = await Home();
                var me = result.links.FirstOrDefault(l => l.rel == Constants.Session.ME);
                if (me != null)
                    return me.href.Substring(me.href.LastIndexOf("/") + 1);
                return "";
            }
            catch (Exception e)
            {
                //log here
                throw;
            }
        }
    }
}
