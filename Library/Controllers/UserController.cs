using Library.Models;
using Library.Models.Dto;
using Library.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser([FromBody] RegisterUserDto dto)
        {
            return Ok(await _mediator.Send(new CreateUserCommand()
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                ConfirmPassword = dto.ConfirmPassword
            }));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateLoginToken([FromBody] LoginDto dto)
        {
            var token = await _mediator.Send(new LoginQuery(dto));
            return Ok(token);
        }

        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return Ok(users);
        }
    }
}