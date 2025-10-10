using System;
using MobiMart.Model;
using MobiMart.ViewModel;
using SQLite;

namespace MobiMart.Service;

public class BusinessService
{
    static SQLiteAsyncConnection? db;
    string baseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5199" : "http://localhost:5198";
    HttpClient client;

    public BusinessService()
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

        await db.CreateTableAsync<Business>();
        await db.CreateTableAsync<MonthlyForecastInstance>();
    }


    public async Task<Business> GetBusinessAsync(int id)
    {
        await Init();
        return await db!.Table<Business>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }


    public async Task<Business> GetBusinessAsync(string code)
    {
        await Init();
        return await db!.Table<Business>().Where(x => x.Code == code).FirstOrDefaultAsync();
    }


    public async Task<bool> BusinessExistsAsync(string code)
    {
        await Init();
        return await db!.Table<Business>().Where(x => x.Code == code).FirstOrDefaultAsync() is not null;
    }


    public async Task UpdateBusinessAsync(Business updatedBusiness)
    {
        await Init();
        await db!.UpdateAsync(updatedBusiness);
    }


    public async Task AddBusinessAsync(Business b)
    {
        await Init();
        await db!.InsertAsync(b);
    }


    public async Task AddMonthlyForecastInstance(MonthlyForecastInstance x)
    {
        await Init();
        await db!.InsertAsync(x);
    }


    public async Task<MonthlyForecastInstance> GetMonthlyForecastInstance()
    {
        await Init();
        var businessId = -1;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        return await db!.Table<MonthlyForecastInstance>().FirstOrDefaultAsync(x => x.BusinessId == businessId); ;
    }
    

    public async Task DeleteMonthlyForecastInstance()
    {
        await Init();
        await db!.DeleteAllAsync<MonthlyForecastInstance>();
    }
}
