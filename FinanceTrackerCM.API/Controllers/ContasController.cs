using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinanceTrackerCM.Application.UseCases.Contas;

namespace FinanceTrackerCM.API.Controllers
{ // Controlador de API para gerenciar as contas financeiras do usuário, responsável por receber as requisições HTTP 
  //relacionadas às contas e delegar a lógica de negócio para os handlers do MediatR
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]// Rota base para as requisições relacionadas às contas (ex: "api/contas")
    public class ContasController : ControllerBase
    {   // Campo para armazenar a instância do MediatR, que será usada para enviar os comandos e consultas relacionados às contas
        private readonly IMediator _mediator;
        // Construtor do controlador que recebe a instância do MediatR por meio de injeção de dependência
        public ContasController(IMediator mediator)
        { // Atribuição da instância do MediatR para uso nos métodos do controlador
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CriarContaCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        { // Método de ação para obter os detalhes de uma conta financeira específica, que recebe o Id da conta como parâmetro
            var query = new ObterContaPorIdQuery { Id = id };
            // Criar uma consulta ObterContaPorIdQuery com o Id da conta a ser obtida
            var result = await _mediator.Send(query);
            // Enviar a consulta ObterContaPorIdQuery para o MediatR, que irá delegar a execução para o handler correspondente (ObterContaPorIdHandler) 
            // e retornar os detalhes da conta solicitada
            if (result == null)
                return NotFound(); // Retornar 404 Not Found se a conta não for encontrada
            return Ok(result); // Retornar 200 OK com os detalhes da conta encontrada
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        { // Método de ação para obter a lista de todas as contas financeiras do usuário
            var query = new ObterTodasContasQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Método de ação para atualizar os dados de uma conta financeira existente, que recebe o Id da conta a ser atualizada como parâmetro na URL e um comando AtualizarContaCommand com os novos dados da conta no corpo da requisição
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AtualizarContaCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        /// <summary>
        ///  Método de ação para excluir uma conta financeira existente, que recebe o Id da conta a ser excluída como parâmetro na URL e envia um comando ExcluirContaCommand para o MediatR, que irá delegar a execução para o handler correspondente (ExcluirContaHandler) e retornar o Id da conta excluída. Retorna 204 No Content se a exclusão for bem-sucedida ou 404 Not Found se a conta não for encontrada.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new ExcluirContaCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}

