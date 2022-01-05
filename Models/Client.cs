using BankSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
