using Library.Commands;
using Library.Entities;
using Library.Exceptions;
using Library.Repositories;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Library.Tests
{
    public class CreateBookCommandHandlerTests
    {
        [Fact]
        public async void CreateBookCommand_CreatesBook_WithValidData()
        {
            var repository = new Mock<IBookRepository>();
            repository.Setup(c => c.AddBook(It.IsAny<Book>(),
                It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var handler = new CreateBookCommandHandler(repository.Object);
            var command = new CreateBookCommand()
            {
                Author = "ja",
                Title = "o mnie",
                Description = "test"
            };
            var book = new Book()
            {
                Title = command.Title,
                Description = command.Description,
                Author = command.Author,
                State = Enums.BookState.Ready,
                Id = 1
            };

            repository.Setup(r => r.AddBook(It.IsAny<Book>(), default))
                .Callback<Book, CancellationToken>((b, ct) =>
                {
                    b.Id = book.Id;
                });

            //
            int result = await handler.Handle(command, default);

            //
            Assert.Equal(book.Id, result);
        }

        [Fact]
        public async void CreateBookCommand_FailsToCreateBook_BookHasNoTitle()
        {
            var book = new Book()
            {
                Title = null,
                Description = "test",
                Author = "ja",
                State = Enums.BookState.Ready,
                Id = 1
            };
            var repository = new Mock<IBookRepository>();
            repository.Setup(c => c.AddBook(It.IsAny<Book>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            repository.Setup(r => r.AddBook(It.IsAny<Book>(), default))
                .Callback<Book, CancellationToken>((b, ct) =>
                {
                    b.Id = book.Id;
                });
            var handler = new CreateBookCommandHandler(repository.Object);

            var command = new CreateBookCommand()
            {
                Author = book.Author,
                Title = book.Title,
                Description = book.Description
            };

            Assert.ThrowsAsync<ForbiddenException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}