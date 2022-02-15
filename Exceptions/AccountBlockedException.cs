using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSimulator.Exceptions
{
    public class AccountBlockedException : Exception
    {
        public AccountBlockedException(string message)
            : base(message) { }
    }
}
