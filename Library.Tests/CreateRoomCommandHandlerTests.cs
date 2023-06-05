using Xunit;
using Library.Entities;
using Library.Repositories;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Library.Commands;
using Library.Exceptions;

namespace Library.Tests
{
    public class CreateRoomCommandHandlerTests
    {
        [Fact]
        public async void CreateRoomCommand_GetsCorrectData_ReturnsSucces()
        {
            var repository = new Mock<IRoomRepository>();
            repository.Setup(r => r.AddRoom(It.IsAny<Room>(),
                It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var handler = new CreateRoomCommandHandler(repository.Object);
            var command = new CreateRoomCommand()
            {
                Name = "sala"
            };
            var room = new Room()
            {
                Name = command.Name,
                Id = 1,
                Reservations = null
            };
            repository.Setup(r => r.AddRoom(It.IsAny<Room>(), default))
                .Callback<Room, CancellationToken>((r, ct) =>
                {
                    r.Id = room.Id;
                });

            int result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(room.Id, result);
        }

        [Fact]
        public async void CreateRoomCommand_GetsIncorrectData_ReturnsError()
        {
            var room = new Room()
            {
                Name = null,
                Id = 1,
                Reservations = null
            };
            var repository = new Mock<IRoomRepository>();
            repository.Setup(r => r.AddRoom(It.IsAny<Room>(),
                It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            repository.Setup(r => r.AddRoom(It.IsAny<Room>(), default))
                .Callback<Room, CancellationToken>((r, ct) =>
                {
                    r.Id = room.Id;
                });
            var handler = new CreateRoomCommandHandler(repository.Object);
            var command = new CreateRoomCommand()
            {
                Name = room.Name
            };

            Assert.ThrowsAsync<ForbiddenException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}