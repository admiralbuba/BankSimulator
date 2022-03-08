using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BankSimulator.Models
{
    public class Bank
    {
        [NotMapped]
        [JsonIgnore]
        public ProcessingCenter ProcessingCenter { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Client>? Clients { get; set; } = null!;
        public Bank() { }
        public Bank(string name)
        {
            Name = name;
        }
        public Bank(string name, ProcessingCenter processingCenter)
        {
            Name = name;
            ProcessingCenter = processingCenter;
        }
        public void RegisterTransaction(Transaction transaction)
        {
            ProcessingCenter.RegisterTransaction(transaction);
        }

        public void RegisterTransaction(string cardNumberFrom, int accountIdto, double sum)
        {
            using ApplicationContext db = new();
            var account = db.Accounts.Where(x => x.Card.CardNumber == cardNumberFrom).FirstOrDefault();
            if (account != null)
                ProcessingCenter.RegisterTransaction(new Transaction { AccountIdFrom = account.Id, AccountIdTo = accountIdto, Sum = sum });
        }
        public void RegisterTransaction(int accountIdfrom, string cardNumberTo, double sum)
        {
            using ApplicationContext db = new();
            var account = db.Accounts.Where(x => x.Card.CardNumber == cardNumberTo).FirstOrDefault();
            if (account != null)
                ProcessingCenter.RegisterTransaction(new Transaction { AccountIdFrom = accountIdfrom, AccountIdTo = account.Id, Sum = sum });
        }
        public void RegisterTransaction(int accountIdfrom, int accountIdTo, double sum)
        {
            ProcessingCenter.RegisterTransaction(new Transaction { AccountIdFrom = accountIdfrom, AccountIdTo = accountIdTo, Sum = sum });
        }
    }
}
