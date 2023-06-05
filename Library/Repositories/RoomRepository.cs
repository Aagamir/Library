using Library.Entities;

namespace Library.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly Context _context;

        public RoomRepository(Context context)
        {
            _context = context;
        }

        public async Task AddRoom(Room room, CancellationToken cancellationToken)
        {
            await _context.Rooms.AddAsync(room, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Room> GetRoom(int id)
        {
            return _context.Rooms.FirstOrDefault(r => r.Id == id);
        }

        public async Task RentRoom(Reservation reservation, CancellationToken cancellationToken)
        {
            await _context.Reservations.AddAsync(reservation, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Reservation> GetReservation(int id)
        {
            return _context.Reservations.FirstOrDefault(c => c.UserId == id);
        }

        public async Task<List<Reservation>> GetAllReservations()
        {
            return _context.Reservations.ToList();
        }
    }
}