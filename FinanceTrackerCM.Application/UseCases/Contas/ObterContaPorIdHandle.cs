using MediatR;
using FinanceTrackerCM.Application.DTOs;
using FinanceTrackerCM.Application.Interfaces;


namespace FinanceTrackerCM.Application.UseCases.Contas;

public class ObterContaPorIdHandle : IRequestHandler<ObterContaPorIdQuery, ContaDto>
{ // Handler para a consulta ObterContaPorIdQuery, responsável por executar a lógica de obtenção dos detalhes de uma conta financeira específica do usuário
    private readonly IAppDbContext _context;
    // Construtor do handler que recebe a instância do contexto de banco de dados por meio de injeção de dependência
    public ObterContaPorIdHandle(IAppDbContext context)
    { // Atribuição da instância do contexto de banco de dados para uso no método Handle
        _context = context;
    }
    // Método Handle que executa a lógica de obtenção dos detalhes de uma conta financeira específica do usuário, utilizando o Id fornecido na consulta ObterContaPorIdQuery e o contexto de banco de dados para recuperar os dados da conta
    public async Task<ContaDto> Handle(ObterContaPorIdQuery request, CancellationToken cancellationToken)
    { // Recuperar a conta do banco de dados com base no Id fornecido na consulta ObterContaPorIdQuery, utilizando o método FindAsync do DbSet de Contas do contexto de banco de dados
        var conta = await _context.Contas.FindAsync(new object[] { request.Id }, cancellationToken);
        if (conta == null)
            throw new ArgumentNullException(nameof(conta), "A conta não pode ser nula."); // Retornar null se a conta não for encontrada no banco de dados

        // Mapear os dados da entidade Conta para um DTO (Data Transfer Object) ContaDto, que será retornado como resultado da consulta
        var contaDto = new ContaDto
        {
            Id = conta.Id,
            Nome = conta.NomeConta,
            Saldo = conta.Saldo,
            Status = conta.Status.ToString(),
            IdUsuario = conta.IdUsuario
        };

        return contaDto; // Retornar o DTO com os detalhes da conta encontrada como resultado da consulta
    }
}