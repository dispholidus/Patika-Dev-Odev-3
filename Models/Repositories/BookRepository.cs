using BookStoreApi.Common;
using BookStoreApi.DbOperations;
using BookStoreApi.Models.Entities;

namespace BookStoreApi.Models.Repositories
{
    // CRUD operations for Book.
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreDbContext _bookStoreDbContext;
        public BookRepository(BookStoreDbContext bookStoreDbContext)
        {
            _bookStoreDbContext = bookStoreDbContext;
        }
        public bool AddBook(CreateBookModel createBookModel)
        {
            Book? book = _bookStoreDbContext.Books.FirstOrDefault(b => b.BookTitle == createBookModel.BookTitle);
            if (book != null)
            {
                return false;
            }
            Book newBook = new()
            {
                BookTitle = createBookModel.BookTitle,
                BookPublishDate = createBookModel.BookPublishDate,
                BookPageCount = createBookModel.BookPageCount,
                GenreId = createBookModel.GenreId
            };

            _bookStoreDbContext.Books.Add(newBook);
            _bookStoreDbContext.SaveChanges();
            return true;
        }

        public bool DeleteBookById(int bookId)
        {
            Book? book = _bookStoreDbContext.Books.FirstOrDefault(b => b.BookId == bookId);
            if (book != null)
            {
                _bookStoreDbContext.Remove(book);
                _bookStoreDbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public BookModel? GetBookById(int bookId)
        {
            Book? book = _bookStoreDbContext.Books.FirstOrDefault(b => b.BookId == bookId);
            if (book != null)
            {
                BookModel bookModel = new()
                {
                    BookTitle = book.BookTitle,
                    Genre = ((GenreEnum)book.GenreId).ToString(),
                    BookPageCount = book.BookPageCount,
                    BookPublishDate = book.BookPublishDate.Date.ToString("dd/MM/yyy")
                };
                return bookModel;
            }
            return null;

        }

        public IEnumerable<BooksModel> GetBooks()
        {
            IEnumerable<Book> books = _bookStoreDbContext.Books.OrderBy(b => b.GenreId).ToList();
            List<BooksModel> booksViewModel = new();
            foreach (var book in books)
            {
                booksViewModel.Add(new BooksModel
                {
                    BookTitle = book.BookTitle,
                    Genre = ((GenreEnum)book.GenreId).ToString(),
                    BookPageCount = book.BookPageCount,
                    BookPublishDate = book.BookPublishDate.Date.ToString("dd/MM/yyy")
                });
            }
            return booksViewModel;
        }

        public bool UpdateBookById(int bookId, UpdateBookModel newBook)
        {
            Book? book = _bookStoreDbContext.Books.FirstOrDefault(b => b.BookId == bookId);
            if (book != null)
            {
                book.BookTitle = newBook.BookTitle != default ? newBook.BookTitle : book.BookTitle;
                book.BookPublishDate = newBook.BookPublishDate != default ? newBook.BookPublishDate : book.BookPublishDate;
                book.GenreId = newBook.GenreId != default ? newBook.GenreId : book.GenreId;
                _bookStoreDbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
    public class BooksModel
    {
        public string BookTitle { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int BookPageCount { get; set; }
        public string BookPublishDate { get; set; } = string.Empty;
    }
    public class CreateBookModel
    {
        public string BookTitle { get; set; } = string.Empty;
        public int GenreId { get; set; }
        public int BookPageCount { get; set; }
        public DateTime BookPublishDate { get; set; }
    }
    public class BookModel
    {
        public string BookTitle { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int BookPageCount { get; set; }
        public string BookPublishDate { get; set; } = string.Empty;
    }
    public class UpdateBookModel
    {
        public string BookTitle { get; set; } = string.Empty;
        public int GenreId { get; set; }
        public int BookPageCount { get; set; }
        public DateTime BookPublishDate { get; set; }
    }

}
