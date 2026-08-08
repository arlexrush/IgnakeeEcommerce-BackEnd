using Ecommerce.Api.AiOrchestration;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public sealed class AiAssistantController : ControllerBase
    {
        private readonly IAiAssistant _assistant;

        public AiAssistantController(IAiAssistant assistant)
        {
            ArgumentNullException.ThrowIfNull(assistant);
            _assistant = assistant;
        }

        [HttpPost("ask", Name = "AskAiAssistant")]
        [ProducesResponseType(typeof(AiAssistantResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.ServiceUnavailable)]
        public async Task<ActionResult<AiAssistantResponse>> AskAsync(
            AiAssistantRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var response = await _assistant.AskAsync(request, User, cancellationToken);
                return Ok(response);
            }
            catch (InvalidOperationException)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable);
            }
        }
    }
}
