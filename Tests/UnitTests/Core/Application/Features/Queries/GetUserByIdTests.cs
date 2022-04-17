using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using Core.Domain.Entities;
using System.Threading;
using System;
using Core.Application.Features.Queries.GetUserById;
using FluentAssertions;

namespace UnitTests.Core.Application.Features.Queries
{
    public class GetUserByIdTests
    {
        [Theory, AutoMoqData]
        public async void GetUserById_UserNotFound_ShouldResponseFailWithNull(Guid userId, Mock<UserManager<AppUser>> userManagerMock)
        {
            AppUser expected = null;

            userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString())).ReturnsAsync(expected);
            GetUserById.Query query = new(userId);
            GetUserById.QueryHandler handler = new(userManagerMock.Object);

            var response = await handler.Handle(query, CancellationToken.None);

            response.Value.Should().BeNull();
            response.Succeeded.Should().BeFalse();
        }

        [Theory, AutoMoqData]
        public async void GetUserById_UserFoundWithId_ShouldResponseSucceeded(Guid userId, Mock<UserManager<AppUser>> userManagerMock)
        {
            AppUser expected = new()
            {
                Id = userId
            };

            userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString())).ReturnsAsync(expected);
            GetUserById.Query query = new(userId);
            GetUserById.QueryHandler handler = new(userManagerMock.Object);

            var response = await handler.Handle(query, CancellationToken.None);

            response.Value.Id.Should().Be(userId);
            response.Succeeded.Should().BeTrue();
        }
    }
}