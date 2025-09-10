using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiMart.Model
{
    internal class ContactInfo
    {
        [PrimaryKey, AutoIncrement]
        public int sContactId { get; set; }
        [ForeignKey(nameof(Supplier))]
        public int supplierId { get; set; } //fk
        public string cName { get; set; } = "";
        public string cEmail { get; set; } = "";
        public string cNumber { get; set; } = "";
        public string cSocials { get; set; } = "";
    }
}
