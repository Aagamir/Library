using Library.Entities;

namespace Library.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }

        public async Task AddUser(User user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<User> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
            return _context.Users.FirstOrDefault(x => x.Email == email);
        }

        public async Task<List<User>> GetUsers(CancellationToken cancellationToken)
        {
            return _context.Users.ToList();
        }

        public async Task<User> GetUser(int userId)
        {
            return _context.Users.FirstOrDefault(c => c.Id == userId);
        }
    }
}