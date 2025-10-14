using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimonMovilidad.Application.Features.Telemetry.CreateTelemetry;

namespace SimonMovilidad.API.Controllers
{
    [Route("api/v1/telemetry")]
    [ApiController]
    public class TelemetrysController : ControllerBase
    {
        private readonly ISender _sender;

        public TelemetrysController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> IngestData(
            [FromBody] CreateTelemetryRequest request,
            CancellationToken cancellationToken
            )
        {
            var command = new CreateTelemetryCommand(
                request.DeviceId,
                request.Timestamp,
                request.Latitude,
                request.Longitude,
                request.FuelLevel,
                request.Temperature);

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
                return StatusCode(StatusCodes.Status400BadRequest, result.Error);

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
