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
        public ApplicationContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=helloapp.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bank>().Ignore(b => b.ProcessingCenter);
        }
    }
}
