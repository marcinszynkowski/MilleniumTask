using MilleniumTask.Models;
using static MilleniumTask.Services.CardService;

namespace MilleniumTask.Helpers
{
    public class CardDetailsTransformHelper
    {
        public static string Transform(string actionName, CardActionPermissionDetails details, CardDetails cardDetails)
        {
            string action = actionName;

            if (details.SupportedCardTypes.Contains(cardDetails.CardType) && details.SupportedStatuses.Contains(cardDetails.CardStatus))
            {

                if (details.CardStatusesRequiringPin != null &&
                    details.CardStatusesRequiringPin.Contains(cardDetails.CardStatus))
                {
                    return action + " (ale jak nie ma PIN to nie)";
                }

                if (details.CardStatusesRequiringEmptyPin != null &&
                    details.CardStatusesRequiringEmptyPin.Contains(cardDetails.CardStatus))
                {
                    return action + " (jeżeli brak PIN)";
                }

                if (details.CardStatusesRequiringPinSet != null &&
                    details.CardStatusesRequiringPinSet.Contains(cardDetails.CardStatus) &&
                    !cardDetails.IsPinSet)
                {
                    return action + " (jeżeli nadany PIN)";
                }
                
                return action;
            }

            return string.Empty;
        }
    }
}
