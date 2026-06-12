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
    }
}

