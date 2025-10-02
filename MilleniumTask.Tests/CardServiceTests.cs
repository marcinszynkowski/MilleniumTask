using MilleniumTask.Interfaces;
using MilleniumTask.Services;
using Moq;
using System.Threading.Tasks;

namespace MilleniumTask.Tests
{
    public class CardServiceTest
    {
        private Mock<ICardService> _cardServiceMock;

        [SetUp]
        public void Setup()
        {
            _cardServiceMock = new Mock<ICardService>();
        }

        [Test]
        public async Task ReturnsCreditCardOnlyAction()
        {
            // Arrange

            var userId = "user123";

            var cards = new Dictionary<string, Dictionary<string, CardService.CardDetails>>
            {
                {
                    userId, new Dictionary<string, CardService.CardDetails>
                    {
                        { "Card123", new CardService.CardDetails("Card123", Enums.CardType.Credit, Enums.CardStatus.Restricted, true) },
                        { "Card124", new CardService.CardDetails("Card124", Enums.CardType.Credit, Enums.CardStatus.Restricted, true) }
                    }
                }
            };

            _cardServiceMock.Setup(s => s.GetCardDetails(userId, "Card123"))
                .ReturnsAsync(cards[userId]["Card123"]);

            // Act

            var resultForCredit = await _cardServiceMock.Object.GetAllowedActionsForCard(userId, "Card123", CancellationToken.None);
            var resultForDebit = await _cardServiceMock.Object.GetAllowedActionsForCard(userId, "Card124", CancellationToken.None);

            // Assert 
            Assert.That(resultForCredit, Is.EquivalentTo(new[] { "ACTION3", "ACTION4", "ACTION5", "ACTION9" }));
            Assert.That(resultForDebit, Is.EquivalentTo(new[] { "ACTION3", "ACTION4", "ACTION9" }));
        }
    }
}