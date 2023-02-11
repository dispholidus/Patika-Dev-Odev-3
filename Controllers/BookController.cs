using AutoMapper;
using BookStoreApi.DbOperations;
using BookStoreApi.Models.Entities;
using BookStoreApi.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public BookController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            IEnumerable<BooksModel> books = _bookRepository.GetBooks(_mapper);
            return Ok(books);
        }

        [HttpGet("{bookId}")]
        public IActionResult GetBookById(int bookId)
        {
            BookModel? book = _bookRepository.GetBookById(bookId, _mapper);
            if (book != null)
            {
                return Ok(book);
            }
            return BadRequest();
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] CreateBookModel book)
        {
            if (_bookRepository.AddBook(book, _mapper))
            {
                return Ok(book);
            }
            return BadRequest();
        }

        [HttpDelete("{bookId}")]
        public IActionResult DeleteBook(int bookId)
        {

            if (_bookRepository.DeleteBookById(bookId))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("{bookId}")]
        public IActionResult UpdateBook(int bookId, UpdateBookModel newBook)
        {
            if (_bookRepository.UpdateBookById(bookId, newBook))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}