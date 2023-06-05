using FluentAssertions;
using Library.Commands;
using Library.Entities;
using Library.Enums;
using Library.Exceptions;
using Library.Repositories;
using Library.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Library.Tests
{
    public class RentBookCommandHandlerTests
    {
        [Fact]
        public async void RentBookCommand_GetsCorrectData_ReturnsSucces()
        {
            var book = new Book()
            {
                Author = "ja",
                Description = "ja",
                Title = "ja",
                State = BookState.Ready,
                Id = 1
            };
            var user = new User()
            {
                Name = "ja",
                Email = "ja",
                Role = Role.User,
                Id = 1,
                Reservation = null,
                RentedBooks = new List<Book>()
            };

            var bookRepository = new Mock<IBookRepository>();
            var userRepository = new Mock<IUserRepository>();
            var userContextService = new Mock<IUserContextService>();
            bookRepository.Setup(x => x.GetBook(
                It.IsAny<int>()))
                .ReturnsAsync(book);
            bookRepository.Setup(x => x.RentBook(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            userRepository.Setup(x => x.GetUser(
                It.IsAny<int>()))
                .ReturnsAsync(user);
            userContextService.Setup(m => m.GetUserId).Returns(1);

            var handler = new RentBookCommandHandler(bookRepository.Object, userContextService.Object, userRepository.Object);
            var command = new RentBookCommand()
            {
                Id = book.Id
            };

            //

            var result = await handler.Handle(command, CancellationToken.None);

            //

            book.State.Should().Be(BookState.Rented);
            book.RentTime.Should().NotBe(null);
            book.UserId.Should().Be(1);
            book.Author.Should().Be(book.Author);
            result.Should().NotBe(null);
        }

        [Fact]
        public async void RentBookCommand_GetsWrongBookId_ReturnsFailure()
        {
            var book = new Book()
            {
                Author = "ja",
                Description = "ja",
                Title = "ja",
                State = BookState.Ready,
                Id = -1
            };
            var user = new User()
            {
                Name = "ja",
                Email = "ja",
                Role = Role.User,
                Id = 1,
                Reservation = null,
                RentedBooks = new List<Book>()
            };
            var bookRepository = new Mock<IBookRepository>();
            var userRepository = new Mock<IUserRepository>();
            var userContextService = new Mock<IUserContextService>();
            bookRepository.Setup(x => x.GetBook(
                It.IsAny<int>()))
                .ReturnsAsync(book);
            bookRepository.Setup(x => x.RentBook(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            userRepository.Setup(x => x.GetUser(
                It.IsAny<int>()))
                .ReturnsAsync(user);
            userContextService.Setup(m => m.GetUserId)
                .Returns(1);

            var handler = new RentBookCommandHandler(bookRepository.Object, userContextService.Object, userRepository.Object);
            var command = new RentBookCommand()
            {
                Id = book.Id
            };

            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}