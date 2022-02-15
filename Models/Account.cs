using BankSimulator.Exceptions;
using System.Text.Json.Serialization;

namespace BankSimulator
{
    public class Account
    {
        public int Id { get; set; }
        public double Sum { get; set; }
        public Card? Card { get; set; }
        public bool IsBlocked { get; set; } = false;
        public int ClientId { get; set; }
        [JsonIgnore]
        public Client Client { get; set; }

        public void TransactTo(int AccountId, int sum)
        {
            var fromId = this.Id;
            this.Client.Bank.RegisterTransaction(fromId, AccountId, sum);
        }
        public void ChargeSum(double sum)
        {
            if (IsBlocked)
                throw new AccountBlockedException($"Аккаунт {Id} заблокирован");
            if (Sum >= sum)
                Sum -= sum;
            else
                throw new ExceededSumException($"Недостаточно средств на счете {Id} для отправки {sum}");
        }
        public void AddSum(double sum)
        {
            if (IsBlocked)
                throw new AccountBlockedException("Аккаунт заблокирован");
            else
                Sum += sum;
        }
    }
}
