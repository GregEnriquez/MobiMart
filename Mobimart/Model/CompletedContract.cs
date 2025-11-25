using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class CompletedContract : SyncEntity
{
    [Indexed]
    public Guid BusinessId { get; set; }
    public string SupplierName { get; set; } = "";
    public DateTimeOffset? ReturnDate { get; set; }
    public DateTimeOffset? DateReturned { get; set; }
    public decimal AmountToPay { get; set; }
    public byte[] ProofImageData { get; set; } = [];
}


public class CompletedContractItem : SyncEntity
{
    [Indexed]
    public Guid ContractId { get; set; }
    public string Name { get; set; } = "";
    public int SoldQuantity { get; set; }
    public int ReturnQuantity { get; set; }
}