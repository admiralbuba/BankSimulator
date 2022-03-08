using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSimulator.Interfaces
{
    public interface IPaymentSystem
    {
        bool TryRegisterTransaction(Card card, string cardNumber, double sum);
    }
}
