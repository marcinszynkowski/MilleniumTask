namespace MilleniumTask.Interfaces
{
    public interface ICardQueryService
    {
        Task<string[]> GetAllowedActionsForCard(string userId, string cardNumber, CancellationToken token);
    }
}
