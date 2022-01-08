using System.Collections.Concurrent;

namespace BankSimulator
{
    public class ProcessingCenter
    {
        public event Action<string>? Notify;
        public Queue<Transaction> TransactionQueue { get; set; } = new();
        public bool IsStarted { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; } = new();
        public void RegisterTransaction(Transaction transaction)
        {
            lock (TransactionQueue)
            {
                using ApplicationContext db = new();
                db.Transactions.Add(transaction);
                db.SaveChanges();
                TransactionQueue.Enqueue(transaction);
                Notify?.Invoke($"Транзакция {db.Transactions.Where(x => x.Id == transaction.Id).FirstOrDefault().Id} зарегистрирована");
            }
        }
        public void Stop()
        {
            IsStarted = false;
            Notify?.Invoke("Центр остановлен");
        }
        public async void StartAsync()
        {
            IsStarted = true;
            Notify?.Invoke("Центр запущен");
            while (IsStarted)
            {
                if(TransactionQueue.Count > 0)
                {
                    var transaction = TransactionQueue.Dequeue();
                    if (transaction != null)
                    {
                        using ApplicationContext db = new();
                        Notify?.Invoke($"Обрабатывается транзакция {db.Transactions.Where(x => x.Id == transaction.Id).FirstOrDefault().Id}");
                        Account? from = db.Accounts.Where(x => x.Id == transaction.AccountIdFrom).FirstOrDefault();
                        Account? to = db.Accounts.Where(x => x.Id == transaction.AccountIdTo).FirstOrDefault();

                        if (from == null || to == null)
                        {
                            if (from == null)
                                Notify?.Invoke($"Счет {transaction.AccountIdFrom} не существует");
                            if (to == null)
                                Notify?.Invoke($"Счет {transaction.AccountIdTo} не существует");
                        }
                        else
                        {
                            if (from.Sum >= transaction.Sum)
                            {
                                from.Sum -= transaction.Sum;
                                to.Sum += transaction.Sum;
                                var trns = db.Transactions.Where(x => x.Id == transaction.Id).FirstOrDefault();
                                if (trns != null)
                                    trns.IsSuccessfull = true;
                                db.SaveChanges();
                                Notify?.Invoke($"Со счета {transaction.AccountIdFrom} поступила сумма {transaction.Sum} на счет {transaction.AccountIdTo}");
                            }
                            else
                            {
                                Notify?.Invoke($"Недостаточно средств на счете {transaction.AccountIdFrom} для отправки {transaction.Sum} на счет {transaction.AccountIdTo}");
                            }
                        }
                    }
                }

            }
        }
    }
}
