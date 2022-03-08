using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSimulator.Exceptions
{

    public class InvalidCardDataException : Exception
    {
        public InvalidCardDataException(string message)
            : base(message) { }
    }

}
