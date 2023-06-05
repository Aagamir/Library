using Library.Entities;
using Library.Exceptions;
using Library.Repositories;
using Library.Services;
using MediatR;

namespace Library.Commands
{
    public class RentRoomCommand : IRequest<DateTime>
    {
        public int roomId { get; set; }
        public DateTime RentTime { get; set; }
        public int Hours { get; set; }
    }

    public class RentRoomCommandHandler : IRequestHandler<RentRoomCommand, DateTime>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IUserContextService _userContextService;
        private readonly IUserRepository _userRepository;

        public RentRoomCommandHandler(IRoomRepository roomRepository, IUserContextService userContextService, IUserRepository userRepository)
        {
            _roomRepository = roomRepository;
            _userContextService = userContextService;
            _userRepository = userRepository;
        }

        public async Task<DateTime> Handle(RentRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetRoom(request.roomId);
            var user = await _userRepository.GetUser((int)_userContextService.GetUserId);
            if (room is null)
            {
                throw new NotFoundException("Wrong room Id");
            }
            if (await _roomRepository.GetReservation(user.Id) != null)
            {
                throw new ForbiddenException("You can only have one reservation");
            }
            user.Reservation = new Reservation();
            var reservations = await _roomRepository.GetAllReservations();

            if (reservations.Any(r => r.RentedFrom > request.RentTime && r.RentedFrom < request.RentTime.AddHours(request.Hours)) || room.Reservations.Any(r => r.RentedUntil > request.RentTime && r.RentedUntil < request.RentTime.AddHours(request.Hours)))
            {
                throw new ForbiddenException("Room already rented at this time");
            }

            user.Reservation.RentedFrom = request.RentTime;
            user.Reservation.RentedUntil = request.RentTime.AddHours(request.Hours);
            user.Reservation.RoomId = room.Id;
            user.Reservation.UserId = user.Id;
            var reservation = user.Reservation;
            await _roomRepository.RentRoom(reservation, cancellationToken);
            return request.RentTime.AddHours(request.Hours);
        }
    }
}