using BankSimulator.Enums;

namespace BankSimulator.Helpers
{
    public static class CurrencyConverter
    {
        public static double USDtoEUR = 0.8968;
        public static double USDtoBYN = 3.0975;
        public static double USDtoGBP = 0.7514;

        public static double EURtoUSD = 1.1151;
        public static double EURtoBYN = 3.454;
        public static double EURtoGBP = 0.8379;

        public static double BYNtoUSD = 0.3228;
        public static double BYNtoEUR = 0.2895;
        public static double BYNtoGBP = 0.2426;

        public static double GBPtoUSD = 1.3308;
        public static double GBPtoEUR = 1.1935;
        public static double GBPtoBYN = 4.1222;

        public static double ConvertCurrency(Currency from, Currency to, int sum)
        {
            if (from == to)
                return sum;
            var currencyRate = typeof(CurrencyConverter).GetField(from.ToString() + "to" + to.ToString(),
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null);

            return sum * (double)currencyRate;
        }
    }
}
