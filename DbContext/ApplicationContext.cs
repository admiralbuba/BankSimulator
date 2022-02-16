using BankSimulator.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSimulator
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Bank> Banks { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Card> Cards { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<Market> Markets { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public ApplicationContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=helloapp.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bank>().Ignore(b => b.ProcessingCenter);
            modelBuilder.Entity<Bank>().HasData(
                    new Bank { Id = 1, Name = "Prior" }
                    );
            modelBuilder.Entity<Client>().HasData(
                    new Client { Name = "Kate", Id = 1, BankId = 1 },
                    new Client { Name = "Mary", Id = 2, BankId = 1 },
                    new Client { Name = "Market", Id = 3, BankId = 1 }
                    );
            modelBuilder.Entity<Account>().HasData(
                    new Account { Id = 1, ClientId = 1, Sum = 50 },
                    new Account { Id = 2, ClientId = 2 },
                    new Account { Id = 3, ClientId = 3 }
                    );
            modelBuilder.Entity<Card>().HasData(
                    new Card { Id = 1, AccountId = 1 },
                    new Card { Id = 2, AccountId = 2 }
                    );
            modelBuilder.Entity<Market>().HasData(
                    new Market { Id = 1, Name = "Coffee", AccountId = 3 }
                    );
            modelBuilder.Entity<Product>().HasData(
                    new Product { Id = 1, Name = "Kawa", Price = 50, Amount = 100, MarketId = 1 },
                    new Product { Id = 2, Name = "Harbata", Price = 25, Amount = 100, MarketId = 1 }
                    );
        }
    }
}
