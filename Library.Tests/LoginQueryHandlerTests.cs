using FluentAssertions;
using Library.Entities;
using Library.Models;
using Library.Models.Dto;
using Library.Queries;
using Library.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using Xunit;

namespace Library.Tests
{
    public class LoginQueryHandlerTests
    {
        [Fact]
        public async void LoginQuery_GetsCorrectData_ReturnsValidationToken()
        {
            var user = new User()
            {
                Id = 1,
                Name = "Test",
                Email = "test@mail.com",
                Role = Enums.Role.User,
                RentedBooks = null,
                Reservation = null,
                PasswordHash = "test"
            };
            var dto = new LoginDto()
            {
                Email = "test@mail.com",
                Password = "test"
            };
            var token = new JwtSecurityToken();

            var passwordHasher = new Mock<IPasswordHasher<User>>();
            var userRepository = new Mock<IUserRepository>();
            var authenticationSettings = new AuthenticationSettings() { JwtIssuer = "tesssssssssssssst", JwtKey = "tessssssssssssssssssssssst" };

            userRepository.Setup(x => x.GetUserByEmail(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            passwordHasher.Setup(x => x.VerifyHashedPassword(
                user, user.PasswordHash, dto.Password))
                .Returns(PasswordVerificationResult.Success);

            var handler = new LoginQueryHandler(passwordHasher.Object, authenticationSettings, userRepository.Object);

            var query = new LoginQuery(dto);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
        }

        [Fact]
        public async void LoginQuery_GetsInvalidPassword_ReturnsError()
        {
            var user = new User()
            {
                Id = 1,
                Name = "Test",
                Email = "test@mail.com",
                Role = Enums.Role.User,
                RentedBooks = null,
                Reservation = null,
                PasswordHash = "test"
            };
            var dto = new LoginDto()
            {
                Email = "test@mail.com",
                Password = "incorrectPassword"
            };
            var token = new JwtSecurityToken();

            var passwordHasher = new Mock<IPasswordHasher<User>>();
            var userRepository = new Mock<IUserRepository>();
            var authenticationSettings = new AuthenticationSettings() { JwtIssuer = "tesssssssssssssst", JwtKey = "tessssssssssssssssssssssst" };

            userRepository.Setup(x => x.GetUserByEmail(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            passwordHasher.Setup(x => x.VerifyHashedPassword(
                user, user.PasswordHash, dto.Password))
                .Returns(PasswordVerificationResult.Failed);

            var handler = new LoginQueryHandler(passwordHasher.Object, authenticationSettings, userRepository.Object);

            var query = new LoginQuery(dto);

            Assert.ThrowsAsync<BadHttpRequestException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}