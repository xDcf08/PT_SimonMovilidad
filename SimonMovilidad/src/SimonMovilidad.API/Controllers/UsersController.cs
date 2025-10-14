using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimonMovilidad.Application.Features.Users.CreateUser;
using SimonMovilidad.Application.Features.Users.GetUserById;
using SimonMovilidad.Application.Features.Users.LoginUser;

namespace SimonMovilidad.API.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserRequest request,
            CancellationToken cancellationToken
            )
        {
            var command = new CreateUserCommand(request.Email, request.Password, request.Role);
            var userId = await _sender.Send(command, cancellationToken);

            if (userId.IsFailure)
                return StatusCode(StatusCodes.Status400BadRequest, userId.Error);

            return CreatedAtAction(nameof(GetUserById), new { id = userId.Value }, null);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var result = await _sender.Send(query);

            if (result.IsFailure)
                return StatusCode(StatusCodes.Status404NotFound, result.Error);

            return StatusCode(StatusCodes.Status200OK, result.Value);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(
            [FromBody] LoginUserRequest request,
            CancellationToken cancellationToken
        )
        {
            var query = new LoginUserQuery(request.Email, request.Password);
            var result = await _sender.Send(query, cancellationToken);

            if (result.IsFailure)
                return StatusCode(StatusCodes.Status401Unauthorized, result.Error);

            return StatusCode(StatusCodes.Status200OK, result.Value);
        }
    }
}
