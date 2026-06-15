using MediatR;
using Microsoft.AspNetCore.Mvc;
using FinanceTrackerCM.Application.UseCases.Contas;

namespace FinanceTrackerCM.API.Controllers
{ // Controlador de API para gerenciar as contas financeiras do usuário, responsável por receber as requisições HTTP 
  //relacionadas às contas e delegar a lógica de negócio para os handlers do MediatR
    [ApiController]
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
        { // Método de ação para criar uma nova conta financeira do usuário, que recebe um comando CriarContaCommand com os dados da conta a ser criada
            var result = await _mediator.Send(command);
            // Enviar o comando CriarContaCommand para o MediatR, que irá delegar a execução para o handler correspondente (CriarContaHandler) 
            // e retornar o resultado (Id da nova conta criada)
            return Ok(result);
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
    }
}

