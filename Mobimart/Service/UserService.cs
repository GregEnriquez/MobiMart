using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using MobiMart.Model;
using Newtonsoft.Json;
using SQLite;

namespace MobiMart.Service;

public class UserService
{
    SQLiteAsyncConnection? db;
    // readonly string baseUrl = "http://10.0.2.2:5198";
    // readonly string baseUrl = "http://localhost:5198";
    readonly string baseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5199" : "http://localhost:5198";
    HttpClient client;

    public UserService()
    {
        client = new HttpClient()
        {
            BaseAddress = new Uri(baseUrl)
        };

    }

    async Task Init()
    {
        if (db != null) return;
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "mobimart.db");
        db = new SQLiteAsyncConnection(databasePath);

        await db.CreateTableAsync<User>();
    }


    // on the register service, redirect the user to the web api register endpoint, then save the result on the local db
    public async Task<bool> RegisterUserAsync(string email, string password)
    {
        // await Init();
        // call web api register
        var payload = new
        {
            Email = email,
            Password = password
        };
        var json = JsonConvert.SerializeObject(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/auth/register", content);
        if (!response.IsSuccessStatusCode)
        {
            Debug.WriteLine(response.Content.ReadAsStringAsync());
            return false;
        }

        // save the user on the database
        // User user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync())!;
        // await db!.InsertAsync(user);

        return true;
    }
}
