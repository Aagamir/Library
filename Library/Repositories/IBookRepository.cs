using Library.Entities;

namespace Library.Repositories
{
    public interface IBookRepository
    {
        Task AddBook(Book book, CancellationToken cancellationToken);

        Task<Book> GetBook(int id);

        Task<List<Book>> GetAllBooks();

        Task RentBook(int bookId, int userId, CancellationToken cancellationToken);

        Task ReturnBook(int bookId, int userId, CancellationToken cancellationToken);
    }
}