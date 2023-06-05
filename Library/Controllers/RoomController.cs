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
    public class RoomController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRoom([FromBody] string name)
        {
            return Ok(await _mediator.Send(new CreateRoomCommand()
            {
                Name = name
            }));
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> RentRoom([FromBody] RentRoomDto dto, [FromRoute] int id)
        {
            return Ok(await _mediator.Send(new RentRoomCommand()
            {
                roomId = id,
                RentTime = dto.RentTime,
                Hours = dto.Hours
            }));
        }

        [HttpGet("reservations")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllReservations()
        {
            return Ok(await _mediator.Send(new GetAllReservationsQuery()));
        }
    }
}