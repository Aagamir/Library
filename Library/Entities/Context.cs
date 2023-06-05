using Microsoft.EntityFrameworkCore;

namespace Library.Entities
{
    public class Context : DbContext
    {
        private string _connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=LibraryDb;Trusted_Connection=True";

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}