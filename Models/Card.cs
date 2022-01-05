
namespace BankSimulator
{
    public class Card
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
        public Card() => CardNumber = new CardNumberGenerator().GenerateCardNumber();

    }
}
