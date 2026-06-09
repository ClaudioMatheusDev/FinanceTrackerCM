namespace FinanceTrackerCM.Domain.Enums
    {
    public enum StatusTransacao
    {
        Pendente, // A transação foi criada, mas ainda não foi processada ou concluída
        Efetivada, // A transação foi concluída com sucesso
        Cancelada // A transação foi cancelada ou estornada antes de ser concluída
    }
}