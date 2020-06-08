﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionStore.Api.DomainEvents.Queries;
using TransactionStore.Api.Models.OData;
using TransactionStore.Api.Models.Transaction;

namespace TransactionStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("odata", Name = "PaymentTransactionsGetFiltered")]
        [ProducesResponseType(typeof(ODataResult<PaymentTransaction>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFiltered([FromQuery] ODataQuery odataQuery)
        {
            var result = await _mediator.Send(new PaymentTransactionsGetFilteredQuery {ODataQuery = odataQuery});
            return Ok(result);
        }
    }
}