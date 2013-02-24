﻿/*
 * Copyright 2012-2013 inBloom, Inc. and its affiliates.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
        internal string CURRENT_ED_ORG_ID = ConfigurationManager.AppSettings["CurrentEdgOrgId"]; //there's no data from SLC about the current user Ed Org, temporarily using a constant value for each environment
    }
}
