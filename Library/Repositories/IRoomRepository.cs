using Library.Entities;

namespace Library.Repositories
{
    public interface IRoomRepository
    {
        Task AddRoom(Room room, CancellationToken cancellationToken);

        Task<Room> GetRoom(int id);

        Task RentRoom(Reservation reservation, CancellationToken cancellationToken);

        Task<Reservation> GetReservation(int id);

        Task<List<Reservation>> GetAllReservations();
    }
}