using Microsoft.AspNetCore.Mvc;
using MilleniumTask.Interfaces;
using MilleniumTask.Services;

namespace MilleniumTask.Controllers
{
    [ApiController]
    [Route("[action]")]
    public class CardActionsController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardActionsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet(Name = "GetAllowedActions")]
        public async Task<string[]> GetAllowedActionsAsync(string userId, string cardNumber, CancellationToken token)
        {
            return await _cardService.GetAllowedActionsForCard(userId, cardNumber, token);
        }
    }
}
