using System.Text.Json.Serialization;

namespace BankSimulator
{
    public class Card
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }

        public int AccountId { get; set; }
        [JsonIgnore]
        public Account Account { get; set; }
        public Card()
        {
            CardNumber = new CardNumberGenerator().GenerateCardNumber();
        }
        public void TransactTo(string cardNumber, int sum)
        {
            var fromId = this.Account.Id;
            Account.Client.Bank.RegisterTransaction(fromId, cardNumber, sum);
        }
    }
}
