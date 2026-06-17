using MediatR;
using FinanceTrackerCM.Domain.Entities;
using FinanceTrackerCM.Domain.Enums;
using FinanceTrackerCM.Application.Interfaces;

namespace FinanceTrackerCM.Application.UseCases.Contas;

public class CriarContaHandle : IRequestHandler<CriarContaCommand, Guid>
{ // Handler para o comando CriarContaCommand, responsável por executar a lógica de criação de uma nova conta financeira do usuário
    private readonly IAppDbContext _context;
    // Construtor do handler que recebe a instância do contexto de banco de dados por meio de injeção de dependência
    private readonly ICurrentUserResolver _currentUserResolver;
    public CriarContaHandle(IAppDbContext context, ICurrentUserResolver currentUserResolver)
    {// Atribuição da instância do contexto de banco de dados para uso no método Handle
        _context = context;
        _currentUserResolver = currentUserResolver;
    }
    // Método Handle que executa a lógica de criação de uma nova conta financeira do usuário, utilizando os dados do comando CriarContaCommand e o contexto de banco de dados para persistir a nova conta
    public async Task<Guid> Handle(CriarContaCommand request, CancellationToken cancellationToken)
    {// Criação de uma nova instância da entidade Conta com os dados fornecidos no comando CriarContaCommand e valores padrão para os campos que não são fornecidos
        var tenantId = _currentUserResolver.TenantId;

        var novaConta = new Conta
        {// Atribuição dos valores para os campos da nova conta, incluindo um novo Guid para o Id, o nome e saldo inicial fornecidos no comando, o status definido como Ativa e um IdUsuario fictício (deve ser substituído pelo Id do usuário autenticado)
            Id = Guid.NewGuid(), // Gerar um novo Guid para o Id da conta
            NomeConta = request.Nome, // Nome da conta fornecido no comando
            Saldo = request.SaldoInicial, // Saldo inicial fornecido no comando
            Status = StatusConta.Ativa, // Status da conta definido como Ativa por padrão
            IdUsuario = _currentUserResolver.UserId, // Id do usuário proprietário da conta (do usuário autenticado)
            TenantId = tenantId,
        };

        _context.Contas.Add(novaConta); // Adicionar a nova conta ao DbSet de Contas do contexto de banco de dados para que ela seja persistida no banco de dados
        await _context.SaveChangesAsync(cancellationToken); // Salvar as alterações no banco de dados de forma assíncrona, garantindo que a nova conta seja persistida

        return novaConta.Id; // Retornar o Id da nova conta criada como resultado do comando, para que o cliente possa ter uma referência à conta recém-criada
    }
}

