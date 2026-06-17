using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using FinanceTrackerCM.Application.UseCases.Transacoes;
namespace FinanceTrackerCM.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly IMediator _mediator;
        // Construtor do controlador que recebe a instância do MediatR por meio de injeção de dependência
        public TransacoesController(IMediator mediator)
        { // Atribuição da instância do MediatR para uso nos métodos do controlador
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CriarTransacaoCommand command)
        { // Método de ação para criar uma nova transação financeira do usuário, que recebe um comando CriarTransacaoCommand com os dados da transação a ser criada
            var result = await _mediator.Send(command);
            // Enviar o comando CriarTransacaoCommand para o MediatR, que irá delegar a execução para o handler correspondente (CriarTransacaoHandler) 
            // e retornar o resultado (Id da nova transação criada)
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        { // Método de ação para obter os detalhes de uma transação financeira específica do usuário, que recebe o ID da transação como parâmetro
            var command = new ObterTransacaoIDCommand { Id = id };
            var result = await _mediator.Send(command);
            // Enviar o comando ObterTransacaoIDCommand para o MediatR, que irá delegar a execução para o handler correspondente (ObterTransacaoIDHandler)
            // e retornar o resultado (detalhes da transação solicitada)
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, AtualizarTransacaoCommand command)
        { // Método de ação para atualizar os dados de uma transação financeira específica do usuário, que recebe o ID da transação e um comando AtualizarTransacaoCommand com os novos dados
            command.Id = id;
            var result = await _mediator.Send(command);
            // Enviar o comando AtualizarTransacaoCommand para o MediatR, que irá delegar a execução para o handler correspondente (AtualizarTransacaoHandler)
            // e retornar o resultado (Id da transação atualizada)
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        { // Método de ação para obter uma lista de todas as transações financeiras do usuário
            var command = new ObterTransacoesCommand();
            var result = await _mediator.Send(command);
            // Enviar o comando ObterTransacoesCommand para o MediatR, que irá delegar a execução para o handler correspondente (ObterTransacoesHandler)
            // e retornar o resultado (lista de transações do usuário)
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        { // Método de ação para excluir uma transação financeira específica do usuário, que recebe o ID da transação a ser excluída como parâmetro

            try
            {
                var command = new ExcluirTransacaoCommand { Id = id };
                var result = await _mediator.Send(command);
                // Enviar o comando ExcluirTransacaoCommand para o MediatR, que irá delegar a execução para o handler correspondente (ExcluirTransacaoHandler)
                // e retornar o resultado (Id da transação excluída)
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}