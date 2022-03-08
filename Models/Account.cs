using BankSimulator.Enums;
using BankSimulator.Exceptions;
using System.Text.Json.Serialization;

namespace BankSimulator
{
    public class Account
    {
        public int Id { get; set; }
        public double Sum { get; set; }
        public Currency Currency { get; set; }
        public Card? Card { get; set; }
        public bool IsBlocked { get; set; } = false;
        public int ClientId { get; set; }
        [JsonIgnore]
        public Client Client { get; set; }

        public void RollbackTransaction(double sum) => Sum += sum;
        public void TransactTo(int AccountId, double sum)
        {
            var fromId = this.Id;
            Client.Bank.RegisterTransaction(fromId, AccountId, sum);
        }
        public bool TryChargeSum(double sum, out string message)
        {
            if (IsBlocked)
            {
                message = $"Аккаунт {Id} заблокирован";
                return false;
            }
            if (Sum >= sum)
            {
                message = $"Со счета {Id} отправлено {sum} в {Currency}";
                Sum -= sum;
                return true;
            }
            else
                throw new ExceededSumException($"Недостаточно средств на счете {Id} для отправки {sum} в {Currency}");
        }
        public bool TryAddSum(double sum, out string message)
        {
            if (IsBlocked)
            {
                message = $"Аккаунт {Id} заблокирован";
                return false;
            }
            else
            {
                message = $"На счет {Id} поступило {sum} в {Currency}";
                Sum += sum;
                return true;
            }
        }
    }
}
