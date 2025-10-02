using MilleniumTask.Enums;

namespace MilleniumTask.Models
{
    public class CardActionPermissionDetails
    {
        public CardStatus[] SupportedStatuses { get; set; } 

        public CardType[] SupportedCardTypes { get; set; } = (CardType[])Enum.GetValues(typeof(CardType));

        public bool IsForCredit { get; set; }

        public bool IsForDebit { get; set; }

        public bool IsForPrepaid { get; set; }

        public bool IsSetPinNeeded { get; set; }

        public CardActionPermissionDetails(CardStatus[] supportedStatuses, CardType[] supportedCardTypes, bool isForCredit, bool isForDebit, bool isForPrepaid, bool isSetPinNeeded)
        {
            SupportedStatuses = supportedStatuses;
            IsForCredit = isForCredit;
            IsForDebit = isForDebit;
            IsForPrepaid = isForPrepaid;
            IsSetPinNeeded = isSetPinNeeded;
            SupportedCardTypes = supportedCardTypes ?? (CardType[])Enum.GetValues(typeof(CardType));
        }

        public CardActionPermissionDetails(CardStatus[] supportedStatuses)
        {
            SupportedStatuses = supportedStatuses;
            IsForCredit = false;
            IsForDebit = false;
            IsForPrepaid = false;
            IsSetPinNeeded = false;
        }


        public CardActionPermissionDetails(CardStatus[] supportedStatuses, CardType[] supportedCardTypes, bool isForCredit, bool isForDebit, bool isForPrepaid)
        {
            SupportedStatuses = supportedStatuses;
            IsForCredit = isForCredit;
            IsForDebit = isForDebit;
            IsForPrepaid = isForPrepaid;
            IsSetPinNeeded = false;
            SupportedCardTypes = supportedCardTypes ?? (CardType[])Enum.GetValues(typeof(CardType));
        }

        public CardActionPermissionDetails(CardStatus[] supportedStatuses, bool isSetPinNeeded)
        {
            SupportedStatuses = supportedStatuses;
            IsForCredit = false;
            IsForDebit = false;
            IsForPrepaid = false;
            IsSetPinNeeded = isSetPinNeeded;
        }
    }
}
