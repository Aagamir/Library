using FluentAssertions;
using Library.Entities;
using Library.Enums;
using Library.Queries;
using Library.Repositories;
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
    public class GetUsersQueryTests
    {
        [Fact]
        public async void GetUsersQuery_ReturnsCorrectResult()
        {
            var user1 = new User()
            {
                Name = "ja1",
                Email = "ja1",
                Role = Role.User,
                Id = 1,
                Reservation = null,
                RentedBooks = new List<Book>()
            };
            var user2 = new User()
            {
                Name = "ja2",
                Email = "ja2",
                Role = Role.User,
                Id = 2,
                Reservation = null,
                RentedBooks = new List<Book>()
            };
            var user3 = new User()
            {
                Name = "ja3",
                Email = "ja3",
                Role = Role.User,
                Id = 3,
                Reservation = null,
                RentedBooks = new List<Book>()
            };
            var users = new List<User>() { user1, user2 };

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetUsers(
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            var handler = new GetUsersQueryHandler(userRepository.Object);
            var query = new GetUsersQuery()
            {
                Users = users
            };

            var result = await handler.Handle(query, CancellationToken.None);

            users.Should().NotBeNullOrEmpty();
        }
    }
}