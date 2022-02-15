using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSimulator.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public List<Market> Markets { get; set; } = new();
        //public Product(string name, int price)
        //{
        //    Name = name;
        //    Price = price;
        //}
    }
}
