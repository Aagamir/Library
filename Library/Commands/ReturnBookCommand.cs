using Library.Enums;
using Library.Exceptions;
using Library.Repositories;
using Library.Services;
using MediatR;

namespace Library.Commands
{
    public class ReturnBookCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, Unit>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserContextService _userContextService;
        private readonly IUserRepository _userRepository;

        public ReturnBookCommandHandler(IBookRepository bookRepository, IUserContextService userContextService, IUserRepository userRepository)
        {
            _bookRepository = bookRepository;
            _userContextService = userContextService;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBook(request.Id);
            var user = await _userRepository.GetUser((int)_userContextService.GetUserId);
            if (book is null)
            {
                throw new NotFoundException("Wrong book Id");
            }
            if (!user.RentedBooks.Contains(book))
            {
                throw new ForbiddenException("Wrong book id");
            }
            book.RentTime = default;
            book.State = BookState.Ready;
            user.RentedBooks.Remove(book);
            //
            _bookRepository.ReturnBook(book.Id, user.Id, cancellationToken);
            return Unit.Value;
        }
    }
}