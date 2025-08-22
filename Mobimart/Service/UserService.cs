using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.Input;
using MobiMart.Model;
using Newtonsoft.Json;
using SQLite;

namespace MobiMart.Service;

public class UserService
{
    static SQLiteAsyncConnection? db;
    string baseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5199" : "http://localhost:5198";
    HttpClient client;

    public UserService()
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
        
        await db.CreateTableAsync<User>();
    }


    public async Task LoginUserAsync(string email, string password)
    {
        await Init();

        var payload = new
        {
            Email = email,
            Password = password
        };
        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/auth/login", content);
    }


    // on the register service, redirect the user to the web api register endpoint, then save the result on the local db
    public async Task<bool> RegisterUserAsync(string email, string password)
    {
        await Init();
        // call web api register
        var payload = new
        {
            Email = email,
            Password = password
        };
        var json = JsonConvert.SerializeObject(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync("/auth/register", content);

            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine(response.Content.ReadAsStringAsync());
                return false;
            }

            // save the user on the database
            User user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync())!;
            var id = await db!.InsertAsync(user);
        }
        catch (HttpRequestException e)
        {
            Debug.WriteLine(e.Message);
            throw new HttpRequestException("Can't connect to the database.");
        }
        catch (TaskCanceledException e)
        {
            Debug.WriteLine(e.Message);
            throw new TaskCanceledException("The server took too long to respond.");
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            throw new Exception(e.Message);
        }

        return true;
    }
}
