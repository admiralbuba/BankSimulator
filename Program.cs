using BankSimulator;
using BankSimulator.Models;
using Microsoft.EntityFrameworkCore;

using (ApplicationContext db = new())
{
    ProcessingCenter pc = new();
    Task.Run(() => pc.Start());
    pc.Notify += Console.WriteLine;

    Bank bank = db.Banks.FirstOrDefault(b => b.Id == 1);
    bank.ProcessingCenter = pc;
    Client? client = db.Clients.FirstOrDefault(c => c.Id == 1);
    Client? client1 = db.Clients.FirstOrDefault(c => c.Id == 2);
    Client? client2 = db.Clients.FirstOrDefault(c => c.Id == 3);
    Account? account = db.Accounts.FirstOrDefault(a => a.Id == 1);
    Account? account1 = db.Accounts.FirstOrDefault(a => a.Id == 2);
    Account? account2 = db.Accounts.FirstOrDefault(a => a.Id == 3);
    Card? card1 = db.Cards.FirstOrDefault(c => c.Id == 1);
    Card? card2 = db.Cards.FirstOrDefault(c => c.Id == 2);
    Market? market = db.Markets.FirstOrDefault(m => m.Id == 1);
    Product? product = db.Products.FirstOrDefault(m => m.Id == 1);
    //market.Products.Add(product);
    //db.SaveChanges();

    foreach(Product prdct in market.Products)
    {
        Console.WriteLine(prdct);
    }
    var cards = db.Cards
        .Include(c => c.Account)
            .ThenInclude(a => a.Client)
                .ThenInclude(cl => cl.Bank)
            .ToList();
    Console.WriteLine("Список карт:");
    foreach (Card card in cards)
    {
        Console.WriteLine($"{card.Account.Client.Name} - {card.CardNumber} в банке {card.Account.Client.Bank.Name}");
    }
    var clients = db.Clients.ToList();
    Console.WriteLine("Деньги на счетах:");
    foreach (Client u in clients)
    {
        Console.WriteLine($"{u.Name} - {u.Id} - {u.Accounts.FirstOrDefault().Sum} ");
    }
    // 1 = 3255351703585185    2 =  1404082264881143
    //pc.Stop();
    //Task.Run(() => pc.StartAsync());
    //card1.TransactTo("1404082264881143", 50);
    //card2.TransactTo("3255351703585185", 100);
    account.TransactTo(2, 50);
    account1.TransactTo(1, 50);
    //market.PayFor("0420521532154173", 50);

}

using (ApplicationContext db = new())
{
    Thread.Sleep(1000);
    var transactions = db.Transactions.ToList();
    Console.WriteLine("Список транзакций:");

    //Parallel.ForEach(transactions, (u) =>
    //{
    //    Console.WriteLine($"{u.Id} - {u.Sum} {u.IsSuccessfull}");
    //});
    foreach (Transaction u in transactions)
    {
        Console.WriteLine($"{u.Id} - {u.Sum} {u.IsSuccessfull}");
    }
}
Console.ReadKey();