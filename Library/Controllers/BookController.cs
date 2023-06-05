using Library.Commands;
using Library.Models.Dto;
using Library.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDto dto)
        {
            return Ok(await _mediator.Send(new CreateBookCommand()
            {
                Title = dto.Title,
                Description = dto.Description,
                Author = dto.Author
            }));
        }

        [HttpPatch("rent/{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> RentBook([FromRoute] int id)
        {
            var rent = await _mediator.Send(new RentBookCommand()
            {
                Id = id
            });
            return Ok($"your book is rented undtil {rent}");
        }

        [HttpPatch("return/{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> ReturnBook([FromRoute] int id)
        {
            return Ok(await _mediator.Send(new ReturnBookCommand()
            {
                Id = id
            }));
        }

        [HttpGet("books")]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok(await _mediator.Send(new GetAllBooksQuery()));
        }
    }
}