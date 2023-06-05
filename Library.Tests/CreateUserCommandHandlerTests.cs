using FluentAssertions;
using Library.Entities;
using Library.Exceptions;
using Library.Queries;
using Library.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Library.Tests
{
    public class CreateUserCommandHandlerTests
    {
        [Fact]
        public async void CreateUserCommand_GetsCorrectData_CreatesNewUser()
        {
            var user = new User()
            {
                Id = 1,
                Name = "Test",
                Email = "test@mail.com",
                Role = Enums.Role.User,
                RentedBooks = null,
                Reservation = null
            };

            var userRepository = new Mock<IUserRepository>();
            var passwordHasher = new Mock<IPasswordHasher<User>>();

            userRepository.Setup(x => x.AddUser(
                It.IsAny<User>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new CreateUserCommandHandler(passwordHasher.Object, userRepository.Object);
            var command = new CreateUserCommand()
            {
                Name = user.Name,
                Email = user.Email,
                Password = "test",
                ConfirmPassword = "test"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            user.Name.Should().Be("Test");
            user.Email.Should().Be("test@mail.com");
            user.Role.Should().Be(Enums.Role.User);
            user.RentedBooks.Should().BeNull();
            user.Reservation.Should().BeNull();
            command.Password.Should().Be("test");
        }

        [Fact]
        public async void CreateUserCommand_IsMissingAnUserName_ReturnsError()
        {
            var user = new User()
            {
                Id = 1,
                Name = null,
                Email = "test@mail.com",
                Role = Enums.Role.User,
                RentedBooks = null,
                Reservation = null
            };

            var userRepository = new Mock<IUserRepository>();
            var passwordHasher = new Mock<IPasswordHasher<User>>();

            userRepository.Setup(x => x.AddUser(
                It.IsAny<User>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new CreateUserCommandHandler(passwordHasher.Object, userRepository.Object);
            var command = new CreateUserCommand()
            {
                Name = user.Name,
                Email = user.Email,
                Password = "test",
                ConfirmPassword = "test"
            };

            Assert.ThrowsAsync<ForbiddenException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}