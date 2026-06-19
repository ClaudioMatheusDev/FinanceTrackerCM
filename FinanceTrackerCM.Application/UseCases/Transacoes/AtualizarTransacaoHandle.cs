using FluentValidation;
using FinanceTrackerCM.Application.Interfaces;
using FinanceTrackerCM.Domain.Entities;
using FinanceTrackerCM.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerCM.Application.UseCases.Transacoes;

public class AtualizarTransacaoHandle : IRequestHandler<AtualizarTransacaoCommand, Guid>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserResolver _currentUserResolver;

    public AtualizarTransacaoHandle(IAppDbContext context, ICurrentUserResolver currentUserResolver)
    {
        _context = context;
        _currentUserResolver = currentUserResolver;
    }

    public async Task<Guid> Handle(AtualizarTransacaoCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _currentUserResolver.TenantId;
        var userId = _currentUserResolver.UserId;

        if (userId == Guid.Empty || tenantId == Guid.Empty)
            throw new UnauthorizedAccessException("Usuario ou tenant nao identificado.");

        var transacao = await _context.Transacoes
            .FirstOrDefaultAsync(t => t.Id == request.Id && t.TenantId == tenantId, cancellationToken);

        if (transacao == null)
            throw new InvalidOperationException("Transacao nao encontrada");

        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == request.CategoriaId && c.TenantId == tenantId, cancellationToken);

        if (categoria == null)
            throw new InvalidOperationException("Categoria nao encontrada para o tenant atual.");

        var contaAnterior = await _context.Contas
            .FirstOrDefaultAsync(c => c.Id == transacao.ContaId && c.TenantId == tenantId, cancellationToken);

        if (contaAnterior == null)
            throw new InvalidOperationException("Conta original da transacao nao encontrada.");

        var novaConta = await _context.Contas
            .FirstOrDefaultAsync(c => c.Id == request.ContaId && c.TenantId == tenantId, cancellationToken);

        if (novaConta == null)
            throw new InvalidOperationException("Conta nao encontrada para o tenant atual.");

        contaAnterior.Saldo -= GetSaldoDelta(transacao.Tipo, transacao.Valor);

        transacao.ContaId = request.ContaId;
        transacao.CategoriaId = request.CategoriaId;
        transacao.Valor = request.Valor;
        transacao.DataTransacao = request.DataTransacao;
        transacao.Descricao = request.Descricao;
        transacao.Tipo = categoria.Tipo;

        new TransacaoValidator().ValidateAndThrow(transacao);

        novaConta.Saldo += GetSaldoDelta(categoria.Tipo, request.Valor);

        await _context.SaveChangesAsync(cancellationToken);
        return transacao.Id;
    }

    private static decimal GetSaldoDelta(TipoTransacao tipo, decimal valor)
    {
        return tipo == TipoTransacao.Receita ? valor : -valor;
    }
}
