using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiMart.Model
{
    public class TransactionRecord
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Payment { get; set; }
        public decimal Change { get; set; }

        public List<Transaction> Items { get; set; } = new List<Transaction>();
    }
}
