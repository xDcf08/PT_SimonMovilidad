using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimonMovilidad.Application.Features.Alerts.GetAlerts;

namespace SimonMovilidad.API.Controllers
{
    [Route("api/v1/alerts")]
    [ApiController]
    public class AlertsController : ControllerBase
    {
        private readonly ISender _sender;

        public AlertsController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetAlerts(CancellationToken cancellationToken)
        {
            var query = new GetAlertsQuery();
            var result = await _sender.Send(query, cancellationToken);

            if (result.IsFailure)
                return StatusCode(StatusCodes.Status404NotFound, result.Error);

            return StatusCode(StatusCodes.Status200OK, result.Value);
        }
    }
}
