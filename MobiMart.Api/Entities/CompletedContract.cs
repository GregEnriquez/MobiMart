using System;

namespace MobiMart.Api.Entities;

public class CompletedContract : SyncEntity
{
    public Guid BusinessId { get; set; }
    
    public string SupplierName { get; set; } = "";
    public DateTimeOffset? ReturnDate { get; set; }
    public DateTimeOffset? DateReturned { get; set; }
    public decimal AmountToPay { get; set; }
    public byte[] ProofImageData { get; set; } = [];
}

public class CompletedContractItem : SyncEntity
{
    public Guid ContractId { get; set; }
    
    public string Name { get; set; } = "";
    public int SoldQuantity { get; set; }
    public int ReturnQuantity { get; set; }
}
