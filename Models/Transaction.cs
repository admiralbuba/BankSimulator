namespace BankSimulator
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountIdFrom { get; set; }
        public int AccountIdTo { get; set; }
        public int Sum { get; set; }
        public bool IsSuccessfull { get; set; } = false;
    }
}
