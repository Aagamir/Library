using Library.Entities;
using Library.Repositories;
using MediatR;

namespace Library.Queries
{
    public class GetUsersQuery : IRequest<List<User>>
    {
        public List<User> Users { get; set; }
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<User>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetUsers(cancellationToken);
            return users;
        }
    }
}