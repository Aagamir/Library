using Library.Entities;
using Library.Enums;
using Microsoft.AspNetCore.Identity;

namespace Library
{
    public class Seeder
    {
        private readonly Context _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public Seeder(Context context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Database.CanConnect())
            {
                if (!_context.Users.Any())
                {
                    var user = GetUser();
                    _context.Users.AddRange(user);
                    _context.SaveChanges();
                }
            }
        }

        private IEnumerable<User> GetUser()
        {
            var user = new List<User>()
            {
                new User()
                {
                    Name = "Admin",
                    Email = "admin@mail.com",
                    PasswordHash = "AQAAAAEAACcQAAAAEN56MrKWxapOO6uTo1MrGF+Z+NVqkXp4+/FapvV0h9Il7zTOvEeUIx5hItADNnIOlw==",
                    Role = Role.Admin,
                    RentedBooks = null
                },
                new User()
                {
                    Name = "User",
                    Email = "user@mail.com",
                    PasswordHash = "AQAAAAEAACcQAAAAEHTWepoeqz/z68vQrf2Fh1fnyHe821JUc36KRidq60eu9tnspawPVmN4dm7F8BKsSw==",
                    Role = Role.User,
                    RentedBooks = null
                }
            };
            return user;
        }
    }
}