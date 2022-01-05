using BankSimulator;
using Microsoft.EntityFrameworkCore;

using (ApplicationContext db = new())
{
    ProcessingCenter pc = new();
    pc.Start();
    pc.Notify += Console.WriteLine;
    //Client client = new() { Name = "Kate" };
    //Client client1 = new() { Name = "Mary" };

    //db.Clients.Add(client);
    //db.Clients.Add(client1);

    //db.Accounts.Add(new Account { Id = 1, ClientId = client.Id, Client = client });
    //db.Accounts.Add(new Account { Id = 2, ClientId = client1.Id, Client = client1 });
    //db.SaveChanges();

    //Account from = db.Accounts.Where(x => x.Id == 1).FirstOrDefault();
    //from.Card = new();
    //Account to = db.Accounts.Where(x => x.Id == 2).FirstOrDefault();
    //to.Card = new();

    //from.Sum = 50;
    //db.SaveChanges();

    var cards = db.Cards
        .Include(c => c.Account)
            .ThenInclude(a => a.Client)
            .ToList();
    Console.WriteLine("Список карт:");
    foreach (Card card in cards)
    {
        Console.WriteLine($"{card.Account.Client.Name} - {card.CardNumber}");
    }

    var clients = db.Clients.ToList();

    Console.WriteLine("Деньги на счетах:");
    foreach (Client u in clients)
    {
        Console.WriteLine($"{u.Id} - {u.Accounts.FirstOrDefault().Sum} ");
    }

    pc.Stop();
    pc.RegisterTransaction(new Transaction { AccountIdFrom = 2, AccountIdTo = 1, Sum = 50 });
    pc.Start();
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