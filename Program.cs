using BankSimulator;
using BankSimulator.Models;
using Microsoft.EntityFrameworkCore;

using (ApplicationContext db = new())
{
    ProcessingCenter pc = new();
    Task.Run(() => pc.StartAsync());
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
    // 1 = 6382022434177845    2 =  2178021011015805
    pc.Stop();
    card1.TransactTo("2178021011015805", 50);
    card2.TransactTo("6382022434177845", 100);
    Task.Run(() => pc.StartAsync());
    //account.TransactTo(2, 50);
    //account1.TransactTo(1, 50);
    //market.PayFor("6382022434177845", 50);

}

using (ApplicationContext db = new())
{
    Thread.Sleep(1000);
    var transactions = db.Transactions.ToList();
    Console.WriteLine("Список транзакций:");
    foreach (Transaction u in transactions)
    {
        Console.WriteLine($"{u.Id} - {u.Sum} {u.IsSuccessfull}");
    }
}
Console.ReadKey();