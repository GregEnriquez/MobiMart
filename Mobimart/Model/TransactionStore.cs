using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiMart.Model
{
    class TransactionStore
    {
        public static ObservableCollection<TransactionRecord> Records { get; set; }
            = new ObservableCollection<TransactionRecord>();
    }
}
