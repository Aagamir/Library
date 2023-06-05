using Library.Entities;
using Library.Enums;
using Library.Exceptions;
using Library.Repositories;
using Library.Services;
using MediatR;

namespace Library.Commands
{
    public class RentBookCommand : IRequest<DateTime>
    {
        public int Id { get; set; }
    }

    public class RentBookCommandHandler : IRequestHandler<RentBookCommand, DateTime>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserContextService _userContextService;
        private readonly IUserRepository _userRepository;

        public RentBookCommandHandler(IBookRepository bookRepository, IUserContextService userContextService, IUserRepository userRepository)
        {
            _bookRepository = bookRepository;
            _userContextService = userContextService;
            _userRepository = userRepository;
        }

        public async Task<DateTime> Handle(RentBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBook(request.Id);
            var user = await _userRepository.GetUser((int)_userContextService.GetUserId);
            if (book is null)
            {
                throw new NotFoundException("Wrong book Id");
            }
            if (user.RentedBooks is null)
            {
                user.RentedBooks = new List<Book>();
            }
            if (user.RentedBooks.Count > 5)
            {
                throw new ForbiddenException("You have too many rented books");
            }
            if (book.State != BookState.Ready)
            {
                throw new ForbiddenException("Book you are looking for is already rented");
            }
            book.State = BookState.Rented;
            book.RentTime = DateTime.Now.AddDays(14);
            book.UserId = user.Id;
            _bookRepository.RentBook(book.Id, user.Id, cancellationToken);
            return book.RentTime;
        }
    }
}