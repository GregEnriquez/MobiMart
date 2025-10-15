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
        var businessId = -1;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        return await db!.Table<Supplier>().Where(x => x.BusinessId == businessId).ToListAsync();
    }


    public async Task AddSupplierAsync(Supplier s)
    {
        await Init();
        await db!.InsertAsync(s);
    }


    public async Task<Supplier> GetSupplierAsync(int id)
    {
        await Init();
        return await db!.Table<Supplier>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }


    public async Task UpdateSupplierAsync(Supplier updatedSupplier)
    {
        await Init();
        await db!.UpdateAsync(updatedSupplier);
    }


    public async Task AddCompletedContractAsync(CompletedContract x)
    {
        await Init();
        await db!.InsertAsync(x);
    }


    public async Task AddCompletedContractItemAsync(CompletedContractItem x)
    {
        await Init();
        await db!.InsertAsync(x);
    }
}
