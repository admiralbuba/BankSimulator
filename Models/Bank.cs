using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSimulator.Models
{
    public class Bank
    {
        [NotMapped]
        public ProcessingCenter ProcessingCenter { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Client>? Clients { get; set; } = null!;
        public Bank(string name)
        {
            Name = name;
        }
        public Bank(string name, ProcessingCenter processingCenter)
        {
            Name = name;
            ProcessingCenter = processingCenter;
        }
        public void RegisterTransaction(int accountIdfrom, string cardNumberTo, int sum)
        {
            using (ApplicationContext db = new())
            {
                var account = db.Accounts.Where(x => x.Card.CardNumber == cardNumberTo).FirstOrDefault();
                ProcessingCenter.RegisterTransaction(new Transaction { AccountIdFrom = accountIdfrom, AccountIdTo = account.Id, Sum = sum });
            }
        }
        public void RegisterTransaction(int accountIdfrom, int accountIdTo, int sum)
        {
            ProcessingCenter.RegisterTransaction(new Transaction { AccountIdFrom = accountIdfrom, AccountIdTo = accountIdTo, Sum = sum });
        }
    }
}
