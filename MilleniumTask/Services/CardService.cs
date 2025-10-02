using Microsoft.VisualBasic;
using MilleniumTask.Enums;
using MilleniumTask.Interfaces;
using MilleniumTask.Models;
using System;

namespace MilleniumTask.Services
{
    public class CardService : ICardService
    {
        public record CardDetails(string CardNumber, CardType CardType, CardStatus CardStatus, bool IsPinSet);
        private readonly Dictionary<string, Dictionary<string, CardDetails>> _userCards = CreateSampleUserCards();
        private readonly Dictionary<string, CardActionPermissionDetails> _allowedActionsForCard = new()
        {
            { "ACTION1", new CardActionPermissionDetails([CardStatus.Active]) },
            { "ACTION2", new CardActionPermissionDetails([CardStatus.Inactive]) },
            { "ACTION3", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Restricted, CardStatus.Blocked, CardStatus.Expired, CardStatus.Closed]) },
            { "ACTION4", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Restricted, CardStatus.Blocked, CardStatus.Expired, CardStatus.Closed]) },
            { "ACTION5", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Restricted, CardStatus.Blocked, CardStatus.Expired, CardStatus.Closed], [CardType.Credit], isForCredit: true, isForDebit: false, isForPrepaid: false) },
            { "ACTION6", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Blocked], isSetPinNeeded: true) },
            { "ACTION7", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Blocked], isSetPinNeeded: true) },
            { "ACTION8", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Blocked]) },
            { "ACTION9", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Restricted, CardStatus.Blocked, CardStatus.Expired, CardStatus.Closed]) },
            { "ACTION10", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active]) },
            { "ACTION11", new CardActionPermissionDetails([CardStatus.Inactive, CardStatus.Active]) },
            { "ACTION12", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active]) },
            { "ACTION13", new CardActionPermissionDetails([CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active]) }
        };

        public async Task<CardDetails?> GetCardDetails(string userId, string cardNumber)
        {
            // At this point, we would typically make an HTTP call to an external service
            // to fetch the data. For this example we use generated sample data.
            await Task.Delay(1000);
            if (!_userCards.TryGetValue(userId, out var cards)
            || !cards.TryGetValue(cardNumber, out var cardDetails))
            {
                return null;
            }
            return cardDetails;
        }
        private static Dictionary<string, Dictionary<string, CardDetails>> CreateSampleUserCards()
        {
            var userCards = new Dictionary<string, Dictionary<string, CardDetails>>();
            for (var i = 1; i <= 3; i++)
            {
                var cards = new Dictionary<string, CardDetails>();
                var cardIndex = 1;
                foreach (CardType cardType in Enum.GetValues(typeof(CardType)))
                {
                    foreach (CardStatus cardStatus in Enum.GetValues(typeof(CardStatus)))
                    {
                        var cardNumber = $"Card{i}{cardIndex}";
                        cards.Add(cardNumber,
                        new CardDetails(
                        CardNumber: cardNumber,
                        CardType: cardType,
                        CardStatus: cardStatus,
                        IsPinSet: cardIndex % 2 == 0));
                        cardIndex++;
                    }
                }
                var userId = $"User{i}";
                userCards.Add(userId, cards);
            }
            return userCards;
        }

        public async Task<string[]> GetAllowedActionsForCard(string userId, string cardNumber, CancellationToken token)
        {
            if (userId is null || cardNumber is null)
            {
                return Array.Empty<string>();
            }

            var data = await GetCardDetails(userId, cardNumber);
            if (data == null)
            {
                return Array.Empty<string>();
            }

            var card = _userCards.Values.Where(y => y.ContainsKey(cardNumber))
                                        .Select(y => y).FirstOrDefault();

            var cardDetails = card != null && card.TryGetValue(cardNumber, out var details) ? details : null;

            if (cardDetails == null)
            {
                return Array.Empty<string>();
            }

            return _allowedActionsForCard
                .Where(y => y.Value.SupportedStatuses.Contains(cardDetails.CardStatus) &&
                            y.Value.SupportedCardTypes.Contains(cardDetails.CardType))
                .Select(y => y.Value.IsSetPinNeeded
                    ? y.Key + " (jeżeli nadany PIN)"
                    : y.Key)
                .ToArray();
        }
    }
}
