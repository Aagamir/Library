using Library.Enums;

namespace Library.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; } = Role.User;

        public List<Book> RentedBooks { get; set; }
        public Reservation? Reservation { get; set; }
    }
}