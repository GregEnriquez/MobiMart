using System;
using MobiMart.Model;
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
}
