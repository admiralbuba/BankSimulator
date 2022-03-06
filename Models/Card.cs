using System.Text.Json.Serialization;

namespace BankSimulator
{
    public class Card
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public int CVV2 { get; set; }
        public DateTime ValidUntil { get; set; }

        public int AccountId { get; set; }
        [JsonIgnore]
        public Account Account { get; set; }
        public Card()
        {
            CardNumber = new CardDataGenerator().GenerateCardNumber();
            CVV2 = new CardDataGenerator().GenerateCVV2();
        }
        public bool TryTransactTo(string cardNumber, int sum, out string message)
        {
            if (DateTime.Now >= ValidUntil)
            {
                message = $"Карта более не действительна";
                return false;
            }
            else
            {
                message = $"Зарегистрировано отправление на карту {cardNumber} на сумму {sum}";
                var fromId = this.Account.Id;
                Account.Client.Bank.RegisterTransaction(fromId, cardNumber, sum);
                return true;
            }
        }
    }
}
