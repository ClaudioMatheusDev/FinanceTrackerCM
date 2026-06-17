using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinanceTrackerCM.Application.UseCases.Categorias;

namespace FinanceTrackerCM.API.Controllers
{
    [ApiController]
    [Authorize]
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

        [HttpGet]
        public async  Task<IActionResult> ListarCategorias()
        {
            var query = new ObterTodasCategoriasCommand();
            var result = await _mediator.Send(query);

            return Ok(result); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirCategoriaHandler(Guid id)
        {
            try
            {
                var command = new ExcluirCategoriaCommand { Id = id };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao excluir a categoria: {ex.Message}");
            }
        }
    }
}