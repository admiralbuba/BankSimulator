using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankSimulator
{
    public class CardNumberGenerator
    {
        public int GenerateCVV2() => new Random().Next(100, 1000);
        public string GenerateCardNumber()
        {
            int[] checkArray = new int[15];

            var cardNum = new int[16];

            for (int d = 14; d >= 0; d--)
            {
                var rnd = new Random();
                cardNum[d] = rnd.Next(0, 9);
                checkArray[d] = (cardNum[d] * (((d + 1) % 2) + 1)) % 9;
            }

            cardNum[15] = (checkArray.Sum() * 9) % 10;

            var sb = new StringBuilder();

            for (int d = 0; d < 16; d++)
            {
                sb.Append(cardNum[d].ToString());
            }
            return sb.ToString();
        }
    }
}
