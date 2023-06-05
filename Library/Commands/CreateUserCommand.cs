using Library.Entities;
using Library.Enums;
using Library.Exceptions;
using Library.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Library.Queries
{
    public class CreateUserCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IPasswordHasher<User> passwordHasher, IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Name is null || request.Email is null || request.Password is null)
            {
                throw new ForbiddenException("User needs to have an Name, Email and Password, your request is missing some of those elements");
            }
            User user = new User();
            user.Name = request.Name;
            user.Email = request.Email;
            user.Role = Role.User;
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            await _userRepository.AddUser(user, cancellationToken);
            return user.Id;
        }
    }
}