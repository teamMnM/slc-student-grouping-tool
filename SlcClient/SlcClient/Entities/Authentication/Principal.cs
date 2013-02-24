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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlcClient.Entities.Authentication
{
    public class Principal
    {
        public string id { get; set; }
        public string name { get; set; }
        public string realm { get; set; }
        public string externalId { get; set; }
        public string adminRealm { get; set; }
        public string edOrg { get; set; }
        public string tenantId { get; set; }
        public string sessionId { get; set; }
        public Entity entity { get; set; }
    }
}
