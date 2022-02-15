using BankSimulator.Exceptions;
using BankSimulator.Extensions;

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
                Notify?.Invoke($"Транзакция {db.Transactions.FirstOrDefault(x => x.Id == transaction.Id).Id} зарегистрирована");
            }
        }
        public void Stop()
        {
            IsStarted = false;
            Notify?.Invoke("Центр остановлен");
        }
        public async void Start()
        {
            IsStarted = true;
            Notify?.Invoke("Центр запущен");
            while (IsStarted)
            {
                if (TransactionQueue.Count > 0)
                {
                    var t = TransactionQueue.Dequeue();
                    using ApplicationContext db = new();
                    Notify?.Invoke($"Обрабатывается транзакция {db.Transactions.FirstOrDefault(x => x.Id == t.Id).Id}");
                    Account? from = db.Accounts.FirstOrDefault(x => x.Id == t.AccountIdFrom);
                    Account? to = db.Accounts.FirstOrDefault(x => x.Id == t.AccountIdTo);

                    if (from is null)
                        Notify?.Invoke($"Счет {t.AccountIdFrom} не существует");
                    if (to is null)
                        Notify?.Invoke($"Счет {t.AccountIdTo} не существует");
                    else
                    {
                        try
                        {
                            from.ChargeSum(t.Sum);
                            to.AddSum(t.Sum);
                            var trns = db.Transactions.FirstOrDefault(x => x.Id == t.Id);
                            if (trns != null)
                                trns.IsSuccessfull = true;
                            db.SaveChanges();
                            Notify?.Invoke($"Со счета {t.AccountIdFrom} поступила сумма {t.Sum} на счет {t.AccountIdTo}");
                        }
                        catch (AccountBlockedException e)
                        {
                            Notify?.Invoke(e.Message);
                        }
                        catch (ExceededSumException e)
                        {
                            Notify?.Invoke(e.Message + $" на счет {t.AccountIdTo}");
                        }
                    }
                }
            }
        }
    }
}
