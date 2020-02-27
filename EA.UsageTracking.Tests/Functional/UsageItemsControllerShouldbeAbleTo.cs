﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EA.UsageTracking.API;
using EA.UsageTracking.Core.DTOs;
using EA.UsageTracking.Core.Entities;
using EA.UsageTracking.SharedKernel.Constants;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace EA.UsageTracking.Tests.Functional
{
    [TestFixture]
    public class UsageItemsControllerShouldBeAbleTo
    {
        private readonly HttpClient _client;

        public UsageItemsControllerShouldBeAbleTo()
        {
            _client = new CustomWebApplicationFactory<Startup>().CreateClient();
            _client.DefaultRequestHeaders.Add(Constants.Tenant.TenantId, "b0ed668d-7ef2-4a23-a333-94ad278f45d7");
        }

        [Test]
        public async Task GetAllForApplication()
        {
            //Act
            var response = await _client.GetAsync("/api/usageItems?PageNumber=1&PageSize=100");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<UsageItemDTO>>(stringResponse).ToList();

            //Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetById()
        {
            //Act
            var response = await _client.GetAsync("/api/usageItems/details?id=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UsageItemDTO>(stringResponse);

            //Assert
            Assert.AreEqual("Application 1", result.ApplicationName);
            Assert.AreEqual("Event 1", result.ApplicationEventName);
            Assert.AreEqual("User 1", result.ApplicationUserName);
        }

        [Test]
        public async Task PostUsageItem()
        {
            //Arrange
            var usageItemDTO = new UsageItemDTO{ApplicationId = 1, ApplicationEventId = 1, ApplicationUserId = 3};

            //Act
            var response = await _client.PostAsJsonAsync("/api/UsageItems", usageItemDTO);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UsageItemDTO>(stringResponse);

            //Assert
            Assert.AreEqual("Application 1", result.ApplicationName);
            Assert.AreEqual("Event 1", result.ApplicationEventName);
            Assert.AreEqual("User 3", result.ApplicationUserName);
        }
    }
}
