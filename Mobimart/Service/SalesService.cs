using System;
using System.Diagnostics;
using MobiMart.Model;
using MobiMart.ViewModel;
using SQLite;

namespace MobiMart.Service;

public class SalesService
{
    static SQLiteAsyncConnection? db;
    string baseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5199" : "http://localhost:5198";
    HttpClient client;

    public SalesService()
    {
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            if (DeviceInfo.DeviceType == DeviceType.Virtual)
                baseUrl = "http://10.0.2.2:5199"; // emulator
            else
                baseUrl = "http://172.20.10.5:5199"; // physical device (replace with server [LAN] IP)
        }
        else
        {
            baseUrl = "http://localhost:5199"; // Windows
        }

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

        await db.CreateTableAsync<SalesTransaction>();
        await db.CreateTableAsync<SalesItem>();
    }


    public async Task AddSalesTransactionAsync(SalesTransaction x)
    {
        await Init();
        x.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.InsertAsync(x);
    }


    public async Task<SalesTransaction> GetSalesTransactionAsync(Guid transactionId)
    {
        await Init();

        var businessId = Guid.Empty;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }

        return await db!.Table<SalesTransaction>().Where(x => x.BusinessId == businessId && x.Id == transactionId && !x.IsDeleted).FirstOrDefaultAsync();
    }


    public async Task UpdateSalesTransactionAsync(SalesTransaction t)
    {
        await Init();
        t.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.UpdateAsync(t);
    }


    public async Task DeleteSalesTransactionAsync(Guid transactionId)
    {
        await Init();
        // soft delete
        var transaction = await GetSalesTransactionAsync(transactionId);
        transaction.IsDeleted = true;
        transaction.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.UpdateAsync(transaction);
        // await db!.DeleteAsync();
    }



    public async Task AddSalesItemAsync(SalesItem x)
    {
        await Init();
        x.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.InsertAsync(x);
    }



    public async Task DeleteSalesItemTransactionAsync(SalesItem x)
    {
        await Init();
        // soft delete
        x.IsDeleted = true;
        x.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.UpdateAsync(x);
        // await db!.DeleteAsync(x);
    }


    public async Task<List<SalesItem>> GetSalesItemsAsync(Guid transactionId)
    {
        await Init();

        return await db!.Table<SalesItem>().Where(x => x.TransactionId == transactionId && !x.IsDeleted).ToListAsync();
    }


    public async Task<List<SalesRecord>> GetSalesRecordsAsync(DateTime date)
    {
        await Init();
        var output = new List<SalesRecord>();
        var businessId = Guid.Empty;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }

        try
        {
            var transactions = (await db!.Table<SalesTransaction>().Where(x => x.BusinessId == businessId  && !x.IsDeleted).ToListAsync())
            .Where(x => x.Date.LocalDateTime.Date == date.Date).ToList();

            foreach (var t in transactions)
            {
                SalesRecord record = new()
                {
                    TransactionId = t.Id,
                    Date = t.Date.LocalDateTime,
                    TotalPrice = t.TotalPrice,
                    Payment = t.Payment,
                    Change = t.Change,
                    Items = []
                };
                var items = await db!.Table<SalesItem>().Where(x => x.TransactionId == t.Id).ToListAsync();

                foreach (var item in items) record.Items.Add(item);

                output.Add(record);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.StackTrace);
        }

        return output;
    }



    public async Task<List<SalesRecord>> GetMonthlySalesRecords(DateTime dateMonth)
    {
        await Init();
        var output = new List<SalesRecord>();
        var businessId = Guid.Empty;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }

        // var ts = await db!.Table<SalesTransaction>().ToListAsync();
        var transactions = (await db!.Table<SalesTransaction>().Where(x => x.BusinessId == businessId && !x.IsDeleted).ToListAsync())
        .Where(x => x.Date.LocalDateTime.Month == dateMonth.Month).ToList();

        foreach (var t in transactions)
        {
            SalesRecord record = new()
            {
                TransactionId = t.Id,
                Date = t.Date.LocalDateTime,
                TotalPrice = t.TotalPrice,
                Payment = t.Payment,
                Change = t.Change,
                Items = []
            };
            var items = await db!.Table<SalesItem>().Where(x => x.TransactionId == t.Id).ToListAsync();

            foreach (var item in items) record.Items.Add(item);

            output.Add(record);
        }


        return output;
    }
}
