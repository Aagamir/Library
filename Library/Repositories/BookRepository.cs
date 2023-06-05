using Library.Entities;

namespace Library.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly Context _context;

        public BookRepository(Context context)
        {
            _context = context;
        }

        public async Task AddBook(Book book, CancellationToken cancellationToken)
        {
            await _context.Books.AddAsync(book, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Book> GetBook(int id)
        {
            return _context.Books.FirstOrDefault(c => c.Id == id);
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return _context.Books.ToList();
        }

        public async Task RentBook(int bookId, int userId, CancellationToken cancellationToken)
        {
            var user = _context.Users.FirstOrDefault(c => c.Id == userId);
            var book = _context.Books.FirstOrDefault(c => c.Id == bookId);
            user.RentedBooks.Add(book);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task ReturnBook(int bookId, int userId, CancellationToken cancellationToken)
        {
            var user = _context.Users.FirstOrDefault(c => c.Id == userId);
            var book = _context.Books.FirstOrDefault(c => c.Id == bookId);
            user.RentedBooks.Remove(book);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}