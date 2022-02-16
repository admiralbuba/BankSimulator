using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BankSimulator.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public int MarketId { get; set; }
        [JsonIgnore]
        public Market Market { get; set; }
    }
}
