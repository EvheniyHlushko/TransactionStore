using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionStore.Api.DomainEvents.Commands;
using TransactionStore.Api.DomainEvents.Notifications;

namespace TransactionStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionFileUploadController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionFileUploadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadFile(IFormFile uploadedFile)
        {
            var result = await _mediator.Send(new UploadTransactionsRequest(uploadedFile));
            await _mediator.Publish(new TransactionsParsed(result.Transactions));
            return Ok();
        }
    }
}