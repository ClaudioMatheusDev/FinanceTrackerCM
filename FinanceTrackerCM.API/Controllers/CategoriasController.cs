using MediatR;
using Microsoft.AspNetCore.Mvc;
using FinanceTrackerCM.Application.UseCases.Categorias;

namespace FinanceTrackerCM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CriarCategoriaHandler([FromBody] CriarCategoriaCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarCategoriaHandler(Guid id, [FromBody] AtualizarCategoriaCommand command)
        {
            if (id != command.Id)
                return BadRequest("O ID da categoria no caminho deve corresponder ao ID no corpo da solicitação.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirCategoriaHandler(Guid id)
        {
            var command = new ExcluirCategoriaCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}