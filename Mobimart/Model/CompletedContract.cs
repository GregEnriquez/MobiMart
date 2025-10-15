using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public class CompletedContract
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [ForeignKey(nameof(Business))]
    public int BusinessId { get; set; }
    public string SupplierName { get; set; } = "";
    public string ReturnDate { get; set; } = "";
    public string DateReturned { get; set; } = "";
    public float AmountToPay { get; set; }
    public byte[] ProofImageData { get; set; } = [];
}


public class CompletedContractItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [ForeignKey(nameof(CompletedContract))]
    public int ContractId { get; set; }
    public string Name { get; set; } = "";
    public int SoldQuantity { get; set; }
    public int ReturnQuantity { get; set; }
}