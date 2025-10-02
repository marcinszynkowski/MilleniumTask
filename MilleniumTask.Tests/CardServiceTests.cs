using MilleniumTask.Interfaces;
using MilleniumTask.Services;
using Moq;
using System.Threading.Tasks;

namespace MilleniumTask.Tests
{
    public class CardServiceTest
    {
        private Mock<ICardService> _cardServiceMock;
        private CardQueryService _cardQueryService;

        [SetUp]
        public void Setup()
        {
            _cardServiceMock = new Mock<ICardService>();
            _cardQueryService = new CardQueryService(_cardServiceMock.Object);
        }

        [Test]
        public async Task ReturnsCreditCardOnlyAction()
        {
            // Arrange

            var userId = "User1";

            var cards = new Dictionary<string, Dictionary<string, CardService.CardDetails>>
            {
                {
                    userId, new Dictionary<string, CardService.CardDetails>
                    {
                        { "Card123", new CardService.CardDetails("Card123", Enums.CardType.Credit, Enums.CardStatus.Restricted, true) },
                        { "Card124", new CardService.CardDetails("Card124", Enums.CardType.Debit, Enums.CardStatus.Restricted, true) }
                    }
                }
            };

            _cardServiceMock.Setup(cs => cs.GetCardsDetails()).Returns(cards);

            _cardServiceMock.Setup(cs => cs.GetCardDetails(userId, "Card123"))
                .ReturnsAsync(new CardService.CardDetails("Card123", Enums.CardType.Credit, Enums.CardStatus.Restricted, true));

            _cardServiceMock.Setup(cs => cs.GetCardDetails(userId, "Card124"))
                .ReturnsAsync(new CardService.CardDetails("Card124", Enums.CardType.Debit, Enums.CardStatus.Restricted, true));

            // Act

            var resultForCredit = await _cardQueryService.GetAllowedActionsForCard(userId, "Card123", CancellationToken.None);
            var resultForDebit = await _cardQueryService.GetAllowedActionsForCard(userId, "Card124", CancellationToken.None);

            // Assert 
            Assert.That(resultForCredit, Is.EquivalentTo(new[] { "ACTION3", "ACTION4", "ACTION5", "ACTION9" }));
            Assert.That(resultForDebit, Is.EquivalentTo(new[] { "ACTION3", "ACTION4", "ACTION9" }));
        }

        [Test]
        public async Task ReturnsActionsWhereSetPinIsNeeded()
        {
            // Arrange
            var userId = "User1";

            var cards = new Dictionary<string, Dictionary<string, CardService.CardDetails>>
            {
                {
                    userId, new Dictionary<string, CardService.CardDetails>
                    {
                        { "Card125", new CardService.CardDetails("Card125", Enums.CardType.Debit, Enums.CardStatus.Active, false) }
                    }
                }
            };

            _cardServiceMock.Setup(cs => cs.GetCardsDetails()).Returns(cards);

            _cardServiceMock.Setup(cs => cs.GetCardDetails(userId, "Card125"))
                .ReturnsAsync(new CardService.CardDetails("Card125", Enums.CardType.Debit, Enums.CardStatus.Active, false));

            // Act

            var resultForPinNotSet = await _cardQueryService.GetAllowedActionsForCard(userId, "Card125", CancellationToken.None);

            // Assert

            Assert.That(resultForPinNotSet, Is.EquivalentTo(new[] { "ACTION1", "ACTION3", "ACTION4", "ACTION6 (ale jak nie ma PIN to nie)", "ACTION7 (je¿eli brak PIN)", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13" }));
        }

        [Test]
        public async Task ReturnsActionsWherePinIsNotNeeded()
        {
            // Arrange
            var userId = "User1";
            var cards = new Dictionary<string, Dictionary<string, CardService.CardDetails>>
            {
                {
                    userId, new Dictionary<string, CardService.CardDetails>
                    {
                        { "Card126", new CardService.CardDetails("Card126", Enums.CardType.Debit, Enums.CardStatus.Active, true) }
                    }
                }
            };
            _cardServiceMock.Setup(cs => cs.GetCardsDetails()).Returns(cards);
            _cardServiceMock.Setup(cs => cs.GetCardDetails(userId, "Card126"))
                .ReturnsAsync(new CardService.CardDetails("Card126", Enums.CardType.Debit, Enums.CardStatus.Active, true));
            // Act
            var resultForPinSet = await _cardQueryService.GetAllowedActionsForCard(userId, "Card126", CancellationToken.None);
            // Assert
            Assert.That(resultForPinSet, Is.EquivalentTo(new[] { "ACTION1", "ACTION3", "ACTION4", "ACTION6 (ale jak nie ma PIN to nie)", "ACTION7 (je¿eli brak PIN)", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13" }));
        }
    }
}