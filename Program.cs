using BankSimulator;
using BankSimulator.Models;
using Microsoft.EntityFrameworkCore;

using (ApplicationContext db = new())
{
    ProcessingCenter pc = new();
    Task.Run(() => pc.Start());
    pc.Notify += Console.WriteLine;

    Bank bank = db.Banks.FirstOrDefault(b => b.Id == 1);
    Client client = db.Clients.FirstOrDefault(c => c.Id == 1);
    Client client1 = db.Clients.FirstOrDefault(c => c.Id == 2);
    Client client2 = db.Clients.FirstOrDefault(c => c.Id == 3);
    Account account = db.Accounts.FirstOrDefault(a => a.Id == 1);
    Account account1 = db.Accounts.FirstOrDefault(a => a.Id == 2);
    Account account2 = db.Accounts.FirstOrDefault(a => a.Id == 3);
    Card card1 = db.Cards.FirstOrDefault(c => c.Id == 1);
    Card card2 = db.Cards.FirstOrDefault(c => c.Id == 2);

    //Market market = new() { Id = 1, Name = "Coffee", AccountId = 3, Account = account2 };
    Market market = db.Markets.FirstOrDefault(m => m.Id == 1);
    //Bank bank = new("Prior", pc);
    //Client client = new() { Name = "Kate", Bank = bank, BankId = bank.Id };
    //Client client1 = new() { Name = "Mary", Bank = bank, BankId = bank.Id };
    //Client client2 = new() { Name = "Market", Bank = bank, BankId = bank.Id };
    //Account account = new() { Id = 1, ClientId = client.Id, Client = client, Card = new(), Sum = 50 };
    //Account account1 = new() { Id = 2, ClientId = client1.Id, Client = client1, Card = new() };
    //Account account2 = new() { Id = 3, ClientId = client2.Id, Client = client2, Card = new() };
    //Card card1 = new() { Account = account, AccountId = account.Id };
    //Card card2 = new() { Account = account1, AccountId = account1.Id };
    //Market market = new() { Id = 1, Name = "Coffee", AccountId = 3, Account = account2 };

    bank.ProcessingCenter = pc;
    //db.Banks.Add(bank);
    //db.Clients.Add(client);
    //db.Clients.Add(client1);
    //db.Accounts.Add(account);
    //db.Accounts.Add(account1);
    //db.Cards.Add(card1);
    //db.Cards.Add(card2);
    //db.SaveChanges();

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
    Task.Run(() => pc.Start());
    account.TransactTo(2, 50);
    account1.TransactTo(1, 50);
    market.PayFor("6382022434177845", 50);
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