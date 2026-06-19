using MediatR;
using FinanceTrackerCM.Application.DTOs;
using FinanceTrackerCM.Application.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace FinanceTrackerCM.Application.UseCases.Contas;

public class ObterContaPorIdHandle : IRequestHandler<ObterContaPorIdQuery, ContaDto>
{ // Handler para a consulta ObterContaPorIdQuery, responsável por executar a lógica de obtenção dos detalhes de uma conta financeira específica do usuário
    private readonly IAppDbContext _context;
    private readonly ICurrentUserResolver _currentUserResolver;
    // Construtor do handler que recebe a instância do contexto de banco de dados por meio de injeção de dependência
    public ObterContaPorIdHandle(IAppDbContext context, ICurrentUserResolver currentUserResolver)
    { // Atribuição da instância do contexto de banco de dados para uso no método Handle
        _context = context;
        _currentUserResolver = currentUserResolver;
    }
    // Método Handle que executa a lógica de obtenção dos detalhes de uma conta financeira específica do usuário, utilizando o Id fornecido na consulta ObterContaPorIdQuery e o contexto de banco de dados para recuperar os dados da conta
    public async Task<ContaDto> Handle(ObterContaPorIdQuery request, CancellationToken cancellationToken)
    { // Recuperar a conta pelo Id e pelo tenant do usuário autenticado
        var tenantId = _currentUserResolver.TenantId;
        var userId = _currentUserResolver.UserId;

        if (userId == Guid.Empty || tenantId == Guid.Empty)
            throw new UnauthorizedAccessException("Usuário ou tenant não identificado.");

        var conta = await _context.Contas
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.TenantId == tenantId, cancellationToken);

        if (conta == null)
            throw new InvalidOperationException("Conta não encontrada");

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
