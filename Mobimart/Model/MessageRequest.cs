using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiMart.Model
{
    public class MessageRequest
    {
        public string itemName { get; set; } = "";
        public int quantity { get; set; }
        public decimal itemPrice { get; set; }
    }
}
