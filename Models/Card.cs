using BankSimulator.Interfaces;
using BankSimulator.Models;
using System.Text.Json.Serialization;

namespace BankSimulator
{
    public class Card : IEquatable<Card?>
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public int CVV2 { get; set; }
        public DateTime ValidUntil { get; set; }
        public IPaymentSystem PaymentSystem { get; set; }

        public int AccountId { get; set; }
        [JsonIgnore]
        public Account Account { get; set; }
        public Card()
        {
            PaymentSystem = new Visa();
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
                return PaymentSystem.TryRegisterTransaction(this, cardNumber, sum);
            }
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Card);
        }

        public bool Equals(Card? other)
        {
            return other != null &&
                   Id == other.Id &&
                   CardNumber == other.CardNumber &&
                   CVV2 == other.CVV2 &&
                   ValidUntil == other.ValidUntil &&
                   AccountId == other.AccountId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, CardNumber, CVV2, ValidUntil, AccountId);
        }
    }
}
