using System;
using MobiMart.Model;
using MobiMart.ViewModel;
using SQLite;

namespace MobiMart.Service;

public class SupplierService
{
    static SQLiteAsyncConnection? db;
    string baseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5199" : "http://localhost:5198";
    HttpClient client;

    public SupplierService()
    {
        client = new HttpClient()
        {
            BaseAddress = new Uri(baseUrl)
        };
        client.Timeout = TimeSpan.FromSeconds(8);
    }


    async Task Init()
    {
        if (db != null) return;
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "mobimart.db");
        db = new SQLiteAsyncConnection(databasePath);

        await db.CreateTableAsync<Supplier>();
        await db.CreateTableAsync<CompletedContract>();
        await db.CreateTableAsync<CompletedContractItem>();
    }


    public async Task<List<Supplier>> GetAllSuppliersAsync()
    {
        await Init();
        var businessId = Guid.Empty;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        return await db!.Table<Supplier>().Where(x => x.BusinessId == businessId && !x.IsDeleted).ToListAsync();
    }


    public async Task<List<CompletedContractItem>> GetCompletedContractItemsAsync(Guid contractId)
    {
        await Init();

        return await db!.Table<CompletedContractItem>().Where(x => x.ContractId == contractId && !x.IsDeleted).ToListAsync();
    }


    public async Task<List<SupplierContract>> GetReturnedSupplierContractsAsync()
    {
        await Init();
        var businessId = Guid.Empty;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }

        var completedContracts = await db!.Table<CompletedContract>().Where(x => x.BusinessId == businessId).ToListAsync();
        if (completedContracts is null || !completedContracts.Any()) return [];

        var output = new List<SupplierContract>();

        foreach (CompletedContract contract in completedContracts)
        {
            var contractItems = new List<ContractItem>();
            var completedContractItems = await GetCompletedContractItemsAsync(contract.Id);
            foreach(var item in completedContractItems)
            {
                contractItems.Add(new()
                {
                   Name = item.Name,
                   ReturnQuantity = item.ReturnQuantity,
                   SoldQuantity = item.SoldQuantity 
                });
            }

            var supContract = new SupplierContract
            {
                SupplierName = contract.SupplierName,
                AmountToPay = contract.AmountToPay,
                HasProof = true,
                ImageData = contract.ProofImageData,
                IsDropped = false,
                IsReturned = true,
                Items = contractItems,
                ImageSource = ImageSource.FromStream(() => new MemoryStream(contract.ProofImageData)),
                ReturnDate = contract.ReturnDate!.Value.LocalDateTime
            };

            output.Add(supContract);
        }

        return output;
    }


    public async Task AddSupplierAsync(Supplier s)
    {
        await Init();
        s.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.InsertAsync(s);
    }


    public async Task<Supplier> GetSupplierAsync(Guid id)
    {
        await Init();
        return await db!.Table<Supplier>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }


    public async Task UpdateSupplierAsync(Supplier updatedSupplier)
    {
        await Init();
        updatedSupplier.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.UpdateAsync(updatedSupplier);
    }


    public async Task AddCompletedContractAsync(CompletedContract x)
    {
        await Init();
        x.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.InsertAsync(x);
    }


    public async Task AddCompletedContractItemAsync(CompletedContractItem x)
    {
        await Init();
        x.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.InsertAsync(x);
    }
}
