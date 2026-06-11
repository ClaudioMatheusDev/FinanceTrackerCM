using MediatR;
using Microsoft.AspNetCore.Mvc;
using FinanceTrackerCM.Application.UseCases.Contas;

namespace FinanceTrackerCM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CriarContaCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

