using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimonMovilidad.Application.Features.Vehicles.CreateVehicle;
using SimonMovilidad.Application.Features.Vehicles.GetLiveLocation;
using SimonMovilidad.Application.Features.Vehicles.GetVehicleHistory;

namespace SimonMovilidad.API.Controllers
{
    [Route("api/v1/vehicles")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly ISender _sender;

        public VehiclesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateVehicle(
            [FromBody] CreateVehicleRequest request,
            CancellationToken cancellationToken
            )
        {
            var command = new CreateVehicleCommand(request.DeviceId, request.LicensePlate, request.AvgConsumption);
            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
                return StatusCode(StatusCodes.Status400BadRequest, result.Error);

            return StatusCode(StatusCodes.Status201Created, result.Value);
        }

        [HttpGet("live")]
        [Authorize]
        public async Task<IActionResult> GetLiveVehicles()
        {
            var query = new GetLiveLocationQuery();
            var result = await _sender.Send(query);

            if (result.IsFailure)
                return StatusCode(StatusCodes.Status404NotFound, result.Error);

            return StatusCode(StatusCodes.Status200OK, result.Value);
        }

        [HttpGet("{vehicleId}/history")]
        [Authorize]
        public async Task<IActionResult> GetVehicleHistory(
            Guid vehicleId,
            [FromQuery] DateTime startTime,
            [FromQuery] DateTime endTime,
            CancellationToken cancellationToken)
        {
            var query = new GetVehicleHistoryQuery(vehicleId, startTime, endTime);
            var result = await _sender.Send(query, cancellationToken);

            if (result.IsFailure)
                return StatusCode(StatusCodes.Status400BadRequest, result.Error);

            return StatusCode(StatusCodes.Status200OK, result.Value);
        }
    }
}
