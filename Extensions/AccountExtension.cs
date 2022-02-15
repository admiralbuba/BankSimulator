using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSimulator.Extensions
{
    public static class AccountExtension
    {
        public static Account IsNull(this Account account, int id) => account is null ? throw new ArgumentNullException(id.ToString()) : account;
    }
}
