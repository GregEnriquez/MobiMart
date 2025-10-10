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
        await db!.InsertAsync(x);
    }



    public async Task AddSalesItemAsync(SalesItem x)
    {
        await Init();
        await db!.InsertAsync(x);
    }



    public async Task<List<SalesRecord>> GetSalesRecordsAsync(DateTime date)
    {
        await Init();
        var output = new List<SalesRecord>();
        int businessId = -1;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }

        try
        {
            var dateString = date.ToString("d");
            var ts = await db!.Table<SalesTransaction>().ToListAsync();
            var transactions = (await db!.Table<SalesTransaction>().Where(x => x.BusinessId == businessId).ToListAsync())
            .Where(x => DateTime.Parse(x.Date).Date == DateTime.Parse(dateString).Date).ToList();

            foreach (var t in transactions)
            {
                SalesRecord record = new()
                {
                    TransactionId = t.Id,
                    Date = t.Date,
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
        int businessId = -1;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }

        var ts = await db!.Table<SalesTransaction>().ToListAsync();
        var transactions = (await db!.Table<SalesTransaction>().Where(x => x.BusinessId == businessId).ToListAsync())
        .Where(x => DateTime.Parse(x.Date).Month == dateMonth.Month).ToList();

        foreach (var t in transactions)
        {
            SalesRecord record = new()
            {
                TransactionId = t.Id,
                Date = t.Date,
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
