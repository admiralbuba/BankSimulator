namespace BankSimulator
{
    public class ProcessingCenter
    {
        public event Action<string>? Notify;
        public Queue<Transaction> TransactionQueue { get; set; } = new();
        public bool IsStopped { get; set; }
        public void RegisterTransaction(Transaction transaction)
        {
            using ApplicationContext db = new();
            db.Transactions.Add(transaction);
            db.SaveChanges();
            TransactionQueue.Enqueue(transaction);
            Notify?.Invoke($"Транзакция {db.Transactions.OrderBy(x => x.Id).Last().Id} зарегистрирована");
        }
        public void Stop()
        {
            IsStopped = false;
            Notify?.Invoke("Центр остановлен");
        }
        public void Start()
        {
            IsStopped = true;
            Notify?.Invoke("Центр запущен");
            Task.Run(() =>
            {
                while (IsStopped)
                {
                    Thread.Sleep(100);
                    if(TransactionQueue.Count > 0)  
                    {
                        var transaction = TransactionQueue.Dequeue();
                        if (transaction != null)
                        {
                            using ApplicationContext db = new();
                            Notify?.Invoke($"Обрабатывается транзакция {db.Transactions.OrderBy(x => x.Id).Last().Id}");
                            Account from = db.Accounts.Where(x => x.Id == transaction.AccountIdFrom).FirstOrDefault();
                            Account to = db.Accounts.Where(x => x.Id == transaction.AccountIdTo).FirstOrDefault();

                            if (from != null && to != null)
                            {
                                if (from.Sum >= transaction.Sum)
                                {
                                    from.Sum -= transaction.Sum;
                                    to.Sum += transaction.Sum;
                                    Notify?.Invoke($"Со счета {transaction.AccountIdFrom} поступила сумма {transaction.Sum} на счет {transaction.AccountIdTo}");
                                    var trns = db.Transactions.OrderBy(x => x.Id).Last();
                                    if (trns != null)
                                        trns.IsSuccessfull = true;

                                    db.SaveChanges();
                                }
                                else
                                {
                                    Notify?.Invoke($"Недостаточно средств на счете {transaction.AccountIdFrom} для отправки {transaction.Sum} на счет {transaction.AccountIdTo}");
                                }
                            }
                        }
                    }
                }
            });
        }
    }
}
