using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSimulator
{
    public class Account
    {
        public int Id { get; set; }
        public double Sum { get; set; }
        public Card? Card { get; set; }
        public bool IsBlocked { get; set; } = false;

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
