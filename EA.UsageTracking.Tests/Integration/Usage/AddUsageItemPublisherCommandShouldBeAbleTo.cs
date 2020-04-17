﻿using System;
using System.Net;
using System.Threading.Tasks;
using EA.UsageTracking.Core.DTOs;
using EA.UsageTracking.Core.Entities;
using EA.UsageTracking.Infrastructure.Features.Usages.Commands;
using EA.UsageTracking.SharedKernel.Constants;
using NUnit.Framework;

namespace EA.UsageTracking.Tests.Integration.Usage
{
    [TestFixture]
    class AddUsageItemPublisherCommandShouldBeAbleTo: BaseIntegration
    {
        private string _identityToken =
            "eyJraWQiOiJoUk1YMXZUQU8zUmpXeHloVG4zZUpSblFZejNpMWZTT044WjV3d0VCVGJrPSIsImFsZyI6IlJTMjU2In0.eyJhdF9oYXNoIjoiVUotcEl0VEZmTElpazJLdFF4VVdtQSIsInN1YiI6IjUzZWZiM2M0LWY4MzctNGUxMS1hNjIyLTYwYTQzYjBlMTJhNCIsImNvZ25pdG86Z3JvdXBzIjpbIkFkbWluIl0sImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJpc3MiOiJodHRwczpcL1wvY29nbml0by1pZHAuZXUtd2VzdC0yLmFtYXpvbmF3cy5jb21cL2V1LXdlc3QtMl9RdGdvZ0g5MXYiLCJjb2duaXRvOnVzZXJuYW1lIjoiNTNlZmIzYzQtZjgzNy00ZTExLWE2MjItNjBhNDNiMGUxMmE0IiwiY29nbml0bzpyb2xlcyI6WyJhcm46YXdzOmlhbTo6MTExMDk0ODE4MjE3OnJvbGVcL0FQSUdhdGV3YXlJbnZva2VSb2xlIl0sImF1ZCI6IjNqMGZla281YWkwYjU4ZDlrOGVjZGp1cGdhIiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE1ODY3ODk1NjcsImV4cCI6MTU4Njc5MzE2NywiaWF0IjoxNTg2Nzg5NTY3LCJlbWFpbCI6ImdyYWhhbS5tY3ZlYUBnbWFpbC5jb20ifQ.BSXjwtmJ4yJf0wcIIr1_JYMng3Yhw6IHrHy0507xO21zzDhWf9wsq2kkvhn0nSMQDTw5sYj6pMAJFNkGMKchk8poT5vgspwhCT-Io1xKVN8a9xVdoxxXJkZrHxv8lw-JoxnC_giLOrvq0XRDE6JsaoLD6EQnxXQKaJ5IzG0784wqMUNOLA7QiHAhFOd5IbvE6zMzbc5jsPIMFRXKF_t71y08CtjYw40wV4Lk8FAvANBwYkPobKzYEZEGNwYQFn3rl5dx4ohJfiennUzVfntDuwh4FELqPSR9dicStUFwBaHvWMdMYSKHRv8GeTpYPY9c2J3JuXrB3z8sicqdt_UhjA";

        private Guid _identityGuid = Guid.Parse("53efb3c4-f837-4e11-a622-60a43b0e12a4");

        [Test]
        public async Task HandleAddUsageItemWithZeroIDEvent()
        {
            // Arrange
            var app = new Core.Entities.Application();
            DbContext.Applications.Add(app);
            DbContext.SaveChanges();

            var command = new AddUsageItemPublisherCommand { ApplicationEventId = 0, IdentityToken = _identityToken};

            // Act
            var result = await Mediator.Send(command);

            //Assert
            Assert.True(result.IsFailure);
            Assert.AreEqual(Constants.ErrorMessages.NoEventExists, result.Error);
        }

        [Test]
        public async Task HandleAddUsageItemWithNoUser()
        {
            // Arrange
            var app = new Core.Entities.Application();
            DbContext.Applications.Add(app);
            var ev = new ApplicationEvent();
            DbContext.ApplicationEvents.Add(ev);
            DbContext.SaveChanges();

            var command = new AddUsageItemPublisherCommand { ApplicationEventId = ev.Id, IdentityToken = string.Empty};

            // Act
            var result = await Mediator.Send(command);

            //Assert
            Assert.True(result.IsFailure);
            Assert.AreEqual(Constants.ErrorMessages.NoIdentityToken, result.Error);
        }

        [Test]
        public async Task AddUsageItem()
        {
            // Arrange
            var app = new Core.Entities.Application();
            DbContext.Applications.Add(app);
            var ev = new ApplicationEvent();
            DbContext.ApplicationEvents.Add(ev);
            DbContext.SaveChanges();

            var command = new AddUsageItemPublisherCommand { ApplicationEventId = ev.Id, IdentityToken = _identityToken};

            // Act
            var result = await Mediator.Send(command);

            //Assert
            Assert.True(result.IsSuccess);
        }

    }
}
