using MilleniumTask.Enums;
using MilleniumTask.Helpers;
using MilleniumTask.Interfaces;
using MilleniumTask.Models;
using System.Security.Cryptography.Xml;
using static MilleniumTask.Services.CardService;

namespace MilleniumTask.Services
{
    public class CardQueryService : ICardQueryService
    {
        private readonly ICardService _cardService;
        private readonly Dictionary<string, CardActionPermissionDetails> _allowedActionsForCard = new()
        {
            { "ACTION1", new CardActionPermissionDetails([CardStatus.Active]) },
            { "ACTION2", new CardActionPermissionDetails([CardStatus.Inactive]) },
            { "ACTION3", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Restricted, CardStatus.Blocked, CardStatus.Expired, CardStatus.Closed]) },
            { "ACTION4", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Restricted, CardStatus.Blocked, CardStatus.Expired, CardStatus.Closed]) },
            { "ACTION5", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Restricted, CardStatus.Blocked, CardStatus.Expired, CardStatus.Closed], [CardType.Credit]) },
            { "ACTION6", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Blocked], null, [CardStatus.Ordered, CardStatus.Active, CardStatus.Inactive], [], [CardStatus.Blocked]) },
            { "ACTION7", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Blocked], null, [], [CardStatus.Ordered, CardStatus.Active, CardStatus.Inactive], [CardStatus.Blocked]) },
            { "ACTION8", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Blocked]) },
            { "ACTION9", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Restricted, CardStatus.Blocked, CardStatus.Expired, CardStatus.Closed]) },
            { "ACTION10", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active]) },
            { "ACTION11", new CardActionPermissionDetails([CardStatus.Inactive, CardStatus.Active]) },
            { "ACTION12", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active]) },
            { "ACTION13", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active]) }
        };

        public CardQueryService(ICardService cardService)
        {
            this._cardService = cardService;
        }

        public async Task<string[]> GetAllowedActionsForCard(string userId, string cardNumber, CancellationToken token)
        {
            if (userId is null || cardNumber is null)
            {
                return Array.Empty<string>();
            }

            var card = await this._cardService.GetCardDetails(userId, cardNumber);

            var data = this._cardService.GetCardsDetails();

            if (data == null)
            {
                return Array.Empty<string>();
            }

            var userCards = this._cardService.GetCardsDetails();

            var cards = userCards.Values.Where(y => y.ContainsKey(cardNumber))
                                        .Select(y => y).FirstOrDefault();

            var cardDetails = cards != null && cards.TryGetValue(cardNumber, out var details) ? details : null;

            if (cardDetails == null)
            {
                return Array.Empty<string>();
            }

            return _allowedActionsForCard
                .Where(x => x.Value.SupportedCardTypes.Contains(cardDetails.CardType))
                .Select(y => CardDetailsTransformHelper.Transform(y.Key, y.Value, cardDetails))
                .Where(z => z != string.Empty)
                .ToArray();
        }
    }
}
