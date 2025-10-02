using static MilleniumTask.Services.CardService;

namespace MilleniumTask.Interfaces
{
    public interface ICardService
    {
        Task<CardDetails?> GetCardDetails(string userId, string cardNumber);

        Dictionary<string, Dictionary<string, CardDetails>> GetCardsDetails();
    }
}
