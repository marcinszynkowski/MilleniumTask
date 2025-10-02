using MilleniumTask.Enums;

namespace MilleniumTask.Models
{
    public class CardActionPermissionDetails
    {
        public CardStatus[] SupportedStatuses { get; set; } 

        public CardType[] SupportedCardTypes { get; set; } = (CardType[])Enum.GetValues(typeof(CardType));

        public CardStatus[] CardStatusesRequiringPin { get; set; }

        public CardStatus[] CardStatusesRequiringEmptyPin { get; set; }

        public CardStatus[] CardStatusesRequiringPinSet { get; set; }

        public CardActionPermissionDetails(CardStatus[] supportedStatuses)
        {
            SupportedStatuses = supportedStatuses;
        }


        public CardActionPermissionDetails(CardStatus[] supportedStatuses, CardType[]? supportedCardTypes = null, CardStatus[]? cardStatusesRequiringPin = null, CardStatus[]? cardStatusesRequiringEmptyPin = null, CardStatus[]? cardStatusesRequiringPinSet = null)
        {
            SupportedStatuses = supportedStatuses;
            SupportedCardTypes = supportedCardTypes ?? (CardType[])Enum.GetValues(typeof(CardType));
            CardStatusesRequiringPin = cardStatusesRequiringPin;
            CardStatusesRequiringEmptyPin = cardStatusesRequiringEmptyPin;
            CardStatusesRequiringPinSet = cardStatusesRequiringPinSet;
        }
    }
}
