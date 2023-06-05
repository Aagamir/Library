using Library.Entities;

namespace Library.Repositories
{
    public interface IUserRepository
    {
        Task AddUser(User user, CancellationToken cancellationToken);

        Task<User> GetUserByEmail(string email, CancellationToken cancellationToken);

        Task<List<User>> GetUsers(CancellationToken cancellationToken);

        Task<User> GetUser(int userId);
    }
}