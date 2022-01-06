using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSimulator.Models
{
    public class Market
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public void PayFor(string cardNumber, int sum)
        {
            var idTo = this.Account.Id;
            this.Account.Client.Bank.RegisterTransaction(cardNumber, idTo, sum);
        }
    }
}
