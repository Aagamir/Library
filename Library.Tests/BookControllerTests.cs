using Library.Controllers;
using Library.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Library.Tests
{
    public class BookControllerTests
    {
        [Fact]
        public async void CreateBook_ReturnsOk()
        {
            var mediator = new Mock<IMediator>();
            var controller = new BookController(mediator.Object);
            var createBookDto = new CreateBookDto
            {
                Title = "Test",
                Description = "Test",
                Author = "Test"
            };

            var result = await controller.CreateBook(createBookDto) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
    }
}