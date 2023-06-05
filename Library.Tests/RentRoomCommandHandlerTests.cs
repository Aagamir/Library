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
    public class RentRoomCommandHandlerTests
    {
        [Fact]
        public async void RentRoomCommand_GetsCorrectData_ReturnsSucces()
        {
            var room = new Room()
            {
                Id = 1,
                Name = "Biblioteka",
                Reservations = new List<Reservation>()
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
            var reservation = new Reservation()

            {
                Id = 1,
                RentedFrom = new DateTime(7777, 07, 07, 07, 07, 07),
                RentedUntil = new DateTime(7777, 07, 07, 07, 07, 07).AddHours(7),
                RoomId = room.Id,
                UserId = user.Id
            };
            var reservations = new List<Reservation>() { reservation };
            var userRepository = new Mock<IUserRepository>();
            var userContextService = new Mock<IUserContextService>();
            var roomRepository = new Mock<IRoomRepository>();

            userRepository.Setup(x => x.GetUser(
               It.IsAny<int>()))
               .ReturnsAsync(user);
            roomRepository.Setup(x => x.GetRoom(
                It.IsAny<int>()))
                .ReturnsAsync(room);
            roomRepository.Setup(x => x.GetReservations(
                It.IsAny<int>()));
            roomRepository.Setup(x => x.GetAllReservations())
                .ReturnsAsync(reservations);
            roomRepository.Setup(x => x.RentRoom(
                It.IsAny<Reservation>(),
                It.IsAny<CancellationToken>()));
            userContextService.Setup(x => x.GetUserId)
                .Returns(1);

            var handler = new RentRoomCommandHandler(roomRepository.Object, userContextService.Object, userRepository.Object);
            var command = new RentRoomCommand()
            {
                roomId = room.Id,
                RentTime = new DateTime(7777, 07, 07, 07, 07, 07),
                Hours = 7
            };
            var result = await handler.Handle(command, CancellationToken.None);

            room.Id.Should().Be(1);
            user.Id.Should().Be(1);
            room.Reservations.Should().NotBeNull();
            user.Reservation.Should().NotBeNull();

            reservation.Id.Should().Be(1);
            reservation.RentedFrom.Should().Be(command.RentTime);
            reservation.RentedUntil.Should().Be(command.RentTime.AddHours(command.Hours));
            reservation.RoomId.Should().Be(room.Id);
            reservation.UserId.Should().Be(user.Id);
        }

        [Fact]
        public async void RentRoomCommand_RoomAlreadyRented_ReturnsError()
        {
            var reservationInit = new Reservation()
            {
                Id = 2,
                RentedFrom = new DateTime(7777, 07, 07, 07, 07, 06),
                RentedUntil = new DateTime(7777, 07, 07, 07, 07, 07).AddHours(7),
                RoomId = 1,
                UserId = 3
            };
            var room = new Room()
            {
                Id = 1,
                Name = "Biblioteka",
                Reservations = new List<Reservation>() { reservationInit }
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
            var reservation = new Reservation()
            {
                Id = 1,
                RentedFrom = new DateTime(7777, 07, 07, 07, 07, 07),
                RentedUntil = new DateTime(7777, 07, 07, 07, 07, 07).AddHours(7),
                RoomId = room.Id,
                UserId = user.Id
            };
            var reservations = new List<Reservation>() { };
            var userRepository = new Mock<IUserRepository>();
            var userContextService = new Mock<IUserContextService>();
            var roomRepository = new Mock<IRoomRepository>();

            userRepository.Setup(x => x.GetUser(
               It.IsAny<int>()))
               .ReturnsAsync(user);
            roomRepository.Setup(x => x.GetRoom(
                It.IsAny<int>()))
                .ReturnsAsync(room);
            roomRepository.Setup(x => x.GetReservations(
                It.IsAny<int>()));
            roomRepository.Setup(x => x.GetAllReservations())
                .ReturnsAsync(reservations);
            roomRepository.Setup(x => x.RentRoom(
                It.IsAny<Reservation>(),
                It.IsAny<CancellationToken>()));
            userContextService.Setup(x => x.GetUserId)
                .Returns(1);

            var handler = new RentRoomCommandHandler(roomRepository.Object, userContextService.Object, userRepository.Object);
            var command = new RentRoomCommand()
            {
                roomId = room.Id,
                RentTime = new DateTime(7777, 07, 07, 07, 07, 07),
                Hours = 7
            };

            Assert.ThrowsAsync<ForbiddenException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}