﻿//
// Copyright (c) Microsoft.  All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Insights.Tests.Helpers;
using Microsoft.Azure.Management.Insights;
using Microsoft.Azure.Management.Insights.Models;
using Xunit;

namespace Insights.Tests.BasicTests
{
    public class LogProfilesTests : TestBase
    {
        private const string ResourceId = "/subscriptions/0e44ac0a-5911-482b-9edd-3e67625d45b5/providers/microsoft.insights/logprofiles/default";

        private static string DefaultName = "default";

        [Fact(Skip = "TODO: fix some serialization issues")]
        public void LogProfiles_CreateOrUpdateTest()
        {
            LogProfileResource expResponse = CreateLogProfile();
            HttpResponseMessage expHttpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(expResponse.ToJson())
            };

            var handler = new RecordedDelegatingHandler(expHttpResponse);
            InsightsManagementClient customClient = this.GetInsightsManagementClient(handler);

            var parameters = CreateLogProfileParams();

            LogProfileResource actualResponse = customClient.LogProfiles.CreateOrUpdate(logProfileName: DefaultName, parameters: parameters);

            AreEqual(expResponse, actualResponse);
        }

        [Fact]
        public void LogProfiles_DeleteTest()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(string.Empty)
            };

            var handler = new RecordedDelegatingHandler(response);
            InsightsManagementClient customClient = this.GetInsightsManagementClient(handler);

            customClient.LogProfiles.Delete(logProfileName: DefaultName);
        }

        [Fact(Skip = "TODO: fix some serialization issues")]
        public void LogProfiles_GetTest()
        {
            var expResponse = CreateLogProfile();
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(expResponse.ToJson())
            };

            var handler = new RecordedDelegatingHandler(response);
            InsightsManagementClient customClient = this.GetInsightsManagementClient(handler);

            LogProfileResource actualResponse = customClient.LogProfiles.Get(logProfileName: DefaultName);
            AreEqual(expResponse, actualResponse);
        }

        [Fact(Skip = "TODO: fix some serialization issues")]
        public void LogProfiles_ListTest()
        {
            var logProfile = CreateLogProfile();
            var expResponse = new List<LogProfileResource>
            {
                logProfile
            };

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(expResponse.ToJson())
            };

            var handler = new RecordedDelegatingHandler(response);
            InsightsManagementClient customClient = this.GetInsightsManagementClient(handler);

            IList<LogProfileResource> actualResponse = customClient.LogProfiles.List().ToList<LogProfileResource>();

            Assert.Equal(expResponse.Count, actualResponse.Count);
            AreEqual(expResponse[0], actualResponse[0]);
        }

        private static LogProfileResource CreateLogProfile()
        {
            return new LogProfileResource
            {
                StorageAccountId = "/subscriptions/4d7e91d4-e930-4bb5-a93d-163aa358e0dc/resourceGroups/Default-Web-westus/providers/microsoft.storage/storageaccounts/sa1",
                ServiceBusRuleId = "/subscriptions/4d7e91d4-e930-4bb5-a93d-163aa358e0dc/resourceGroups/Default-Web-westus/providers/microsoft.servicebus/namespaces/sb1/authorizationrules/ar1",
                Categories = new List<string> { "Delete", "Write" },
                Locations = new List<string> { "global", "eastus" },
                RetentionPolicy = new RetentionPolicy
                {
                    Days = 4,
                    Enabled = true,
                }
            };
        }

        private static LogProfileCreateOrUpdateParameters CreateLogProfileParams()
        {
            return new LogProfileCreateOrUpdateParameters
            {
                StorageAccountId = "/subscriptions/4d7e91d4-e930-4bb5-a93d-163aa358e0dc/resourceGroups/Default-Web-westus/providers/microsoft.storage/storageaccounts/sa1",
                ServiceBusRuleId = "/subscriptions/4d7e91d4-e930-4bb5-a93d-163aa358e0dc/resourceGroups/Default-Web-westus/providers/microsoft.servicebus/namespaces/sb1/authorizationrules/ar1",
                Categories = new List<string> { "Delete", "Write" },
                Locations = new List<string> { "global", "eastus" },
                RetentionPolicy = new RetentionPolicy
                {
                    Days = 4,
                    Enabled = true,
                }
            };
        }

        private static void AreEqual(LogProfileResource exp, LogProfileResource act)
        {
            if (exp != null)
            {
                CompareListString(exp.Categories, act.Categories);
                CompareListString(exp.Locations, act.Locations);

                Assert.Equal(exp.RetentionPolicy.Enabled, act.RetentionPolicy.Enabled);
                Assert.Equal(exp.RetentionPolicy.Days, act.RetentionPolicy.Days);
                Assert.Equal(exp.ServiceBusRuleId, act.ServiceBusRuleId);
                Assert.Equal(exp.StorageAccountId, act.StorageAccountId);
            }
        }

        private static void CompareListString(IList<string> exp, IList<string> act)
        {
            if (exp == act)
            {
                return;
            }

            if (exp == null)
            {
                Assert.Equal(null, act);
            }

            Assert.False(act == null, "List can't be null");

            for (int i = 0; i < exp.Count; i++)
            {
                if (i >= act.Count)
                {
                    Assert.Equal(exp.Count, act.Count);
                }

                string cat1 = exp[i];
                string cat2 = act[i];
                Assert.Equal(cat1, cat2);
            }

            Assert.Equal(exp.Count, act.Count);
        }
    }
}
