using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeamMnMGroupingWebApp.Controllers
{
    public class BaseController : Controller
    {
        internal string SLC_API_SANDBOX_URL = ConfigurationManager.AppSettings["SlcApiSandboxUrl"];
        internal string SLC_CLIENT_ID = ConfigurationManager.AppSettings["SlcClientId"];
        internal string SLC_SHARED_SECRET = ConfigurationManager.AppSettings["SlcSharedSecret"];
        internal string SLC_REDIRECT_URL = ConfigurationManager.AppSettings["SlcRedirectUrl"];
        internal string SLC_SANDBOX_LOGIN = ConfigurationManager.AppSettings["SlcSandboxLogin"];
        internal string SLC_OAUTH_URL = ConfigurationManager.AppSettings["SlcOAuthUrl"];
    }
}
