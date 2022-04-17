using Core.Application.Features.Commands.CreateUser;
using UnitTests.Extensions.Core.Application;
using Core.Application.MapperProfiles;
using Core.Application.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Core.Domain.Entities;
using AutoFixture.Xunit2;
using System.Threading;
using FluentAssertions;
using AutoMapper;
using Xunit;
using Moq;

namespace UnitTests.Core.Application.Features.Commands
{
    public class CreateUserTests
    {
        private readonly IMapper _mapper;
        public CreateUserTests()
        {
            MapperConfiguration mapperConfiguration = new(cfg =>
            {
                cfg.AddProfile<UsersProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        [Theory, AutoMoqData]
        public async void CreateUser_CreateUserFailure_ShouldResponseFailWithNullResultAndOneIdentityError(
            Mock<UserManager<AppUser>> userManagerMock,
            [Frozen] ILogger<CreateUser.CommandHandler> logger,
            [Frozen] IEmailConfirmationLinkSender emailConfirmationLinkSender)
        {
            var command = UserExtensions.GetCreateUserCommand();
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), command.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError()
                {
                    Code = "Username",
                    Description = "Username already exists"
                }));
            CreateUser.CommandHandler handler = new(_mapper, userManagerMock.Object, logger, emailConfirmationLinkSender);

            var response = await handler.Handle(command, CancellationToken.None);

            response.Value.Should().BeNull();
            response.Succeeded.Should().BeFalse();
            response.Errors.Should().HaveCount(1);
        }

        [Theory, AutoMoqData]
        public async void CreateUser_UsernameAlreadyExistsAndInvalidEmail_ShouldReturnTwoValidationErrors(
            Mock<UserManager<AppUser>> userManagerMock)
        {
            var command = UserExtensions.GetCreateUserCommand()
                .WithEmail("invalid");

            userManagerMock.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync(default(AppUser));
            userManagerMock.Setup(x => x.FindByNameAsync(command.Username)).ReturnsAsync(new AppUser());

            var validationResult = await new CreateUser.CommandValidator(userManagerMock.Object).ValidateAsync(command);

            validationResult.Errors.Should().HaveCount(2);
        }
    }
}