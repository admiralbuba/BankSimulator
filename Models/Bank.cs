using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSimulator.Models
{
    public class Bank
    {
        private ProcessingCenter processingCenter;
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Client>? Clients { get; set; } = null!;
        public Bank(string name)
        {
            Name = name;
        }
        public void RegisterTransaction(int accountIdfrom, string cardNumberTo, int sum, ProcessingCenter center)
        {
            using (ApplicationContext db = new())
            {
                var account = db.Accounts.Where(x => x.Card.CardNumber == cardNumberTo).FirstOrDefault();
                center.RegisterTransaction(new Transaction { AccountIdFrom = accountIdfrom, AccountIdTo = account.Id, Sum = sum });
            }
        }
        public void RegisterTransaction(int accountIdfrom, int accountIdTo, int sum, ProcessingCenter center)
        {
            center.RegisterTransaction(new Transaction { AccountIdFrom = accountIdfrom, AccountIdTo = accountIdTo, Sum = sum });
        }
    }
}
