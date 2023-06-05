using Library.Entities;
using Library.Repositories;
using MediatR;

namespace Library.Queries
{
    public class GetAllReservationsQuery : IRequest<List<Reservation>>
    {
        public List<Reservation> Reservations { get; set; }
    }

    public class GetAllReservationsQueryHandler : IRequestHandler<GetAllReservationsQuery, List<Reservation>>
    {
        private readonly IRoomRepository _roomRepository;

        public GetAllReservationsQueryHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<List<Reservation>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
        {
            var reservations = await _roomRepository.GetAllReservations();
            return reservations;
        }
    }
}