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
using Xunit;

namespace Library.Tests
{
    public class ReturnBookCommandTests
    {
        [Fact]
        public async void ReturnBookCommand_RecivesCorrectData_ReturnsSucces()
        {
            var book = new Book()
            {
                Author = "ja",
                Description = "ja",
                Title = "ja",
                State = BookState.Rented,
                RentTime = DateTime.Now.AddDays(18),
                Id = 1
            };
            var user = new User()
            {
                Name = "ja",
                Email = "ja",
                Role = Role.User,
                Id = 1,
                Reservation = null,
                RentedBooks = new List<Book>() { book }
            };

            var bookRepository = new Mock<IBookRepository>();
            var userRepository = new Mock<IUserRepository>();
            var userContextService = new Mock<IUserContextService>();
            bookRepository.Setup(x => x.GetBook(
                It.IsAny<int>()))
                .ReturnsAsync(book);
            userRepository.Setup(x => x.GetUser(
               It.IsAny<int>()))
               .ReturnsAsync(user);
            bookRepository.Setup(x => x.ReturnBook(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()));
            userContextService.Setup(x => x.GetUserId).Returns(1);

            var handler = new ReturnBookCommandHandler(bookRepository.Object, userContextService.Object, userRepository.Object);
            var command = new ReturnBookCommand()
            {
                Id = book.Id
            };

            var result = await handler.Handle(command, CancellationToken.None);

            book.State.Should().Be(BookState.Ready);
            book.RentTime.Should().Be(default);
            book.UserId.Should().Be(null);
            book.Author.Should().Be(book.Author);
            user.RentedBooks.Should().NotContain(book);
        }

        [Fact]
        public async void ReturnBookCommand_UserTriesToReturnWrongBook_ReturnsError()
        {
            var book = new Book()
            {
                Author = "ja",
                Description = "ja",
                Title = "ja",
                State = BookState.Rented,
                RentTime = DateTime.Now.AddDays(18),
                Id = 1
            };
            var book2 = new Book()
            {
                Author = "ja2",
                Description = "ja2",
                Title = "ja2",
                State = BookState.Rented,
                RentTime = DateTime.Now.AddDays(18),
                Id = 2
            };
            var user = new User()
            {
                Name = "ja",
                Email = "ja",
                Role = Role.User,
                Id = 1,
                Reservation = null,
                RentedBooks = new List<Book>() { book }
            };

            var bookRepository = new Mock<IBookRepository>();
            var userRepository = new Mock<IUserRepository>();
            var userContextService = new Mock<IUserContextService>();
            bookRepository.Setup(x => x.GetBook(
                It.IsAny<int>()))
                .ReturnsAsync(book2);
            userRepository.Setup(x => x.GetUser(
               It.IsAny<int>()))
               .ReturnsAsync(user);
            bookRepository.Setup(x => x.ReturnBook(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()));
            userContextService.Setup(x => x.GetUserId).Returns(1);

            var handler = new ReturnBookCommandHandler(bookRepository.Object, userContextService.Object, userRepository.Object);
            var command = new ReturnBookCommand()
            {
                Id = 2
            };

            Assert.ThrowsAsync<ForbiddenException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}