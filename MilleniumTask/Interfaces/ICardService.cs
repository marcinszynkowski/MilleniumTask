using static MilleniumTask.Services.CardService;

namespace MilleniumTask.Interfaces
{
    public interface ICardService
    {
        Task<string[]> GetAllowedActionsForCard(string userId, string cardNumber, CancellationToken token);

        Task<CardDetails?> GetCardDetails(string userId, string cardNumber);
    }
}
