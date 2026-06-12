namespace FinanceTrackerCM.Domain.Enums
{
    public enum StatusTransacao
    {// Enumeração que representa o status de uma transação financeira do usuário
        Pendente, // A transação foi criada, mas ainda não foi processada ou concluída
        Efetivada, // A transação foi concluída com sucesso
        Cancelada // A transação foi cancelada ou estornada antes de ser concluída
    }
}