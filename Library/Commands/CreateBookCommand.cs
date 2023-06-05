using Library.Entities;
using Library.Enums;
using Library.Exceptions;
using Library.Repositories;
using MediatR;

namespace Library.Commands
{
    public class CreateBookCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
    }

    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, int>
    {
        private readonly IBookRepository _bookRepository;

        public CreateBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<int> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            if (request.Title is null || request.Description is null || request.Author is null)
            {
                throw new ForbiddenException("Book needs to have an Title, Description and Author, your request is missing some of those elements");
            }

            Book book = new Book();
            book.Title = request.Title;
            book.Description = request.Description;
            book.Author = request.Author;
            book.State = BookState.Ready;
            await _bookRepository.AddBook(book, cancellationToken);
            return book.Id;
        }
    }
}