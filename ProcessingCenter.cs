using BankSimulator.Exceptions;
using BankSimulator.Extensions;

namespace BankSimulator
{
    public class ProcessingCenter
    {
        public event Action? TransactionCompleted;
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
                Notify?.Invoke($"Транзакция {db.Transactions.FirstOrDefault(x => x.Id == transaction.Id).Id} зарегистрирована");
            }
        }
        public void Stop()
        {
            IsStarted = false;
            Notify?.Invoke("Центр остановлен");
        }
        public void Start()
        {
            IsStarted = true;
            Notify?.Invoke("Центр запущен");
            while (IsStarted)
            {
                if (TransactionQueue.Count > 0)
                {
                    var transaction = TransactionQueue.Dequeue();
                    using ApplicationContext db = new();
                    Notify?.Invoke($"Обрабатывается транзакция {transaction.Id}");
                    Account? from = db.Accounts.FirstOrDefault(x => x.Id == transaction.AccountIdFrom);
                    Account? to = db.Accounts.FirstOrDefault(x => x.Id == transaction.AccountIdTo);

                    if (from is null)
                        Notify?.Invoke($"Счет {transaction.AccountIdFrom} не существует");
                    if (to is null)
                        Notify?.Invoke($"Счет {transaction.AccountIdTo} не существует");
                    else
                    {
                        try
                        {
                            if (from.TryChargeSum(transaction.Sum, out string message))
                            {
                                if (to.TryAddSum(transaction.Sum, out string msg))
                                {
                                    Transaction? trns = db.Transactions.FirstOrDefault(x => x.Id == transaction.Id);
                                    trns.IsSuccessfull = true;
                                    db.SaveChanges();
                                    Notify?.Invoke(message);
                                    Notify?.Invoke(msg);
                                    TransactionCompleted?.Invoke();
                                }
                                else
                                    Notify?.Invoke(msg);
                            }
                            else
                                Notify?.Invoke(message);
                        }
                        catch (ExceededSumException e)
                        {
                            Notify?.Invoke(e.Message + $" на счет {transaction.AccountIdTo}");
                        }
                    }
                }
            }
        }
    }
}
