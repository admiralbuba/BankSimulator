using BankSimulator.Models;
using System.Text.Json.Serialization;

namespace BankSimulator
{
    public class Client
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Account>? Accounts { get; set; } = null!;

        public int BankId { get; set; }
        [JsonIgnore]
        public Bank Bank { get; set; }
    }
}
