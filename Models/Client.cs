using BankSimulator.Models;
namespace BankSimulator
{
    public class Client
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Account>? Accounts { get; set; } = null!;

        public int BankId { get; set; }
        public Bank Bank { get; set; }
    }
}
