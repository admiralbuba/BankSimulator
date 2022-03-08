using BankSimulator.Exceptions;
using BankSimulator.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSimulator.Models
{
    public class Visa : IPaymentSystem
    {
        public Queue<Transaction> TransactionQueue { get; set; } = new();
        private CancellationTokenSource CancellationTokenSource { get; set; } = new();
        public CancellationToken CancellationToken { get; set; }
        public Visa()
        {
            CancellationToken = CancellationTokenSource.Token;
        }
        public bool TryRegisterTransaction(Card card, string cardNumber, double sum)
        {
            using ApplicationContext db = new();
            var dbCard = db.Cards.FirstOrDefault(x => x.Id == card.Id);
            if (dbCard is null)
                throw new InvalidCardDataException($"Карты {card.Id} нет в базе Visa");
            if (card.Equals(dbCard))
            {
                card.Account.Client.Bank.RegisterTransaction(card.AccountId, cardNumber, sum);
                Log.Information($"Visa обработала транзакцию с карты {card.CardNumber} на карту {cardNumber}");
                return true;
            }
            else
                return false;
        }
    }
}
