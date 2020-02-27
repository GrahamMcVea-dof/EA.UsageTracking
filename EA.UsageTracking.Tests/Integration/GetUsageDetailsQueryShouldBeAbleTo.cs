﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using EA.UsageTracking.Core.DTOs;
using EA.UsageTracking.Core.Entities;
using EA.UsageTracking.Core.Queries;
using EA.UsageTracking.Infrastructure.Commands;
using EA.UsageTracking.Infrastructure.Data;
using EA.UsageTracking.SharedKernel.Constants;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace EA.UsageTracking.Tests.Integration
{
    [TestFixture]
    public class GetUsageDetailsQueryShouldBeAbleTo
    {
        private readonly UsageTrackingContext _dbContext;
        private readonly IUsageTrackingContextFactory _dbContextFactory;

        public GetUsageDetailsQueryShouldBeAbleTo()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[Constants.Tenant.TenantId])
                .Returns("b0ed668d-7ef2-4a23-a333-94ad278f45d7");
 
            _dbContextFactory = new UsageTrackingContextFactory(httpContextAccessorMock.Object, Helper.CreateNewContextOptionsUsingInMemoryDatabase());
            _dbContext = _dbContextFactory.UsageTrackingContext;
        }

        [TearDown]
        public void CleanUp()
        {
            _dbContext.PurgeTable(_dbContext.UsageItems);
            _dbContext.PurgeTable(_dbContext.ApplicationUsers);
            _dbContext.PurgeTable(_dbContext.ApplicationEvents);
            _dbContext.PurgeTable(_dbContext.Applications);
            _dbContext.SaveChangesAsync().Wait();
        }

        [Test]
        public void GetUsageDetails()
        {
            //Arrange
            var app = new Application();
            _dbContext.Applications.Add(app);
            var ev = new ApplicationEvent();
            _dbContext.ApplicationEvents.Add(ev);
            var user = new ApplicationUser();
            _dbContext.ApplicationUsers.Add(user);
            _dbContext.SaveChanges();

            var item = new UsageItemDTO { ApplicationId = app.Id, ApplicationEventId = ev.Id, ApplicationUserId = user.Id };
            var addUsageItemCommandHandler = new AddUsageItemCommandHandler(_dbContextFactory);
            var getUsageDetailsQueryHandler = new GetUsageDetailsQueryHandler(_dbContextFactory);

            //Act
            var addResult = addUsageItemCommandHandler.Handle(new AddUsageItemCommand() { UsageItemDTO = item },
                CancellationToken.None);

            var result = getUsageDetailsQueryHandler.Handle(new GetUsageDetailsQuery {Id = addResult.Result.Value.Id},
                CancellationToken.None);

            //Assert
            Assert.True(result.Result.IsSuccess);
        }

        [Test]
        public void HandleNoUsageDetails()
        {
            //Arrange
            var getUsageDetailsQueryHandler = new GetUsageDetailsQueryHandler(_dbContextFactory);

            //Act
            var result = getUsageDetailsQueryHandler.Handle(new GetUsageDetailsQuery { Id = 1 },
                CancellationToken.None);

            //Assert
            Assert.True(result.Result.IsFailure);
            Assert.AreEqual(Constants.ErrorMessages.NoItemExists, result.Result.Error);
        }
    }
}
