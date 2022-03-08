using BankSimulator.Exceptions;
using BankSimulator.Extensions;
using BankSimulator.Helpers;
using Serilog;

namespace BankSimulator
{
    public class ProcessingCenter
    {
        public event Action? TransactionCompleted;
        public AsyncQueue<Transaction> TransactionQueue { get; set; } = new();
        private CancellationTokenSource CancellationTokenSource { get; set; } = new();
        public CancellationToken CancellationToken { get; set; }
        public ProcessingCenter()
        {
            CancellationToken = CancellationTokenSource.Token;
        }
        public void RegisterTransaction(Transaction transaction)
        {
            lock (TransactionQueue)
            {
                using ApplicationContext db = new();
                db.Transactions.Add(transaction);
                db.SaveChanges();
                Log.Information($"Транзакция {db.Transactions.FirstOrDefault(x => x.Id == transaction.Id).Id} зарегистрирована");
                TransactionQueue.Enqueue(transaction);
            }
        }
        public void Stop() => CancellationTokenSource.Cancel();
        public async void StartAsync()
        {
            Log.Information("Центр запущен");
            while (!CancellationToken.IsCancellationRequested)
            {
                var transaction = await TransactionQueue.DequeueAsync();
                using ApplicationContext db = new();
                Log.Information("Обрабатывается транзакция {@transaction}", transaction);
                Account? from = db.Accounts.FirstOrDefault(x => x.Id == transaction.AccountIdFrom);
                Account? to = db.Accounts.FirstOrDefault(x => x.Id == transaction.AccountIdTo);

                if (from is null)
                    Log.Warning($"Счет {transaction.AccountIdFrom} не существует");
                if (to is null)
                    Log.Warning($"Счет {transaction.AccountIdTo} не существует");
                else
                {
                    try
                    {
                        if (from.TryChargeSum(transaction.Sum, out string message))
                        {
                            var sum = CurrencyConverter.ConvertCurrency(from.Currency, to.Currency, transaction.Sum);
                            if (to.TryAddSum(sum, out string msg))
                            {
                                Transaction? trns = db.Transactions.FirstOrDefault(x => x.Id == transaction.Id);
                                trns.IsSuccessfull = true;
                                db.SaveChanges();
                                Log.Information($"{message}. Id Транзакции: {transaction.Id}");
                                Log.Information($"{msg}. Id Транзакции: {transaction.Id}");
                                Log.Information("Транзакция успешно выполнена {@transaction}", trns);
                                TransactionCompleted?.Invoke();
                            }
                            else
                            {
                                from.RollbackTransaction(transaction.Sum);
                                Log.Information($"{msg}. Id Транзакции: {transaction.Id}");
                            }
                        }
                        else
                            Log.Information($"{message}. Id Транзакции: {transaction.Id}");
                    }
                    catch (ExceededSumException e)
                    {
                        Log.Warning(e.Message + $" на счет {transaction.AccountIdTo}");
                    }
                }
            }
            Log.Information("Центр остановлен");
        }
    }
}
