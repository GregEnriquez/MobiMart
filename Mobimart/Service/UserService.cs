using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
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
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            if (DeviceInfo.DeviceType == DeviceType.Virtual)
                baseUrl = "http://10.0.2.2:5199"; // emulator
            else
                baseUrl = "https://app-mobimart-dev-southeastasia-01.azurewebsites.net"; // physical device (replace with server [LAN] IP)
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

        await db.CreateTableAsync<User>();
        await db.CreateTableAsync<UserInstance>();
    }


    public async Task<bool> ResumeUserInstanceAsync()
    {
        await Init();

        // check for an active user instance
        UserInstance userInstance = null;
        userInstance = await db!.Table<UserInstance>().FirstOrDefaultAsync();

        if (userInstance is null)
        {
            // return the user to login page
            return false;
        }

        // if refresh token validity is expired
        DateTime expiry = DateTime.Parse(userInstance.RefreshTokenExpiryTime);
        if (expiry < DateTime.Today)
        {
            // logout the user
            await LogoutUserAsync();
            return false;
        }

        // else, try to refresh the tokens (online)
        var user = await db!.Table<User>().Where(x => x.Id == userInstance.UserId).FirstOrDefaultAsync();
        var payload = new
        {
            UserId = user.Id,
            RefreshToken = user.RefreshToken
        };
        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        try
        {
            var response = await client.PostAsync("/auth/refresh-token", content);
            if (!response.IsSuccessStatusCode)
            {
                if (response.ReasonPhrase == "Unauthorized")
                {
                    // logout bro
                    await LogoutUserAsync();
                    return false;
                }
            }
            var tokens = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
            user.RefreshToken = tokens!["refreshToken"];
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss.fff");
            await db.UpdateAsync(user);

            userInstance.AccessToken = tokens!["accessToken"];
            userInstance.RefreshToken = user.RefreshToken;
            userInstance.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;
            await db.UpdateAsync(userInstance);
        }
        // if can't connect online, just let bro in anyway (since refresh token is not expired)
        catch (HttpRequestException e)
        {
            Debug.WriteLine(e.StackTrace);
        }
        catch (TaskCanceledException e)
        {
            Debug.WriteLine(e.StackTrace);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.StackTrace);
        }

        // attach authorization header to the http client
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userInstance.AccessToken);
        // take the user to the main page
        return true;
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
        var tokens = new Dictionary<string, string>();
        try
        {
            var response = await client.PostAsync("/auth/login", content);
            if (!response.IsSuccessStatusCode) throw new Exception("Invalid username or password.");

            tokens = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens!["accessToken"]);
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
            Debug.WriteLine(e.StackTrace);
            throw new Exception(e.Message);
        }

        // check if user already exists on the local db
        var user = await db!.Table<User>().FirstOrDefaultAsync(x => x.Email == email);
        if (user is null)
        {
            // pull user from remote db
            var response = await client.GetAsync($"users/{email}");
            user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync())!;
            // then add it to local db
            await db!.InsertAsync(user);
        }

        // replace tokens
        user = await db.Table<User>().Where(x => x.Id == user!.Id).FirstOrDefaultAsync();
        user.RefreshToken = tokens["refreshToken"];
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss.fff");
        await db.UpdateAsync(user);

        // create user instance
        await db!.InsertAsync(new UserInstance()
        {
            UserId = user.Id,
            AccessToken = tokens["accessToken"],
            RefreshToken = user.RefreshToken,
            RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
        });
        // debug
        // var instance = await db.Table<UserInstance>().Where(x => x.UserId == user!.Id).FirstOrDefaultAsync();
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


    public async Task LogoutUserAsync()
    {
        await Init();

        var userInstance = await db!.Table<UserInstance>().FirstOrDefaultAsync();
        var user = await db!.Table<User>().Where(x => x.Id == userInstance.UserId).FirstOrDefaultAsync();

        user.RefreshToken = "";
        user.RefreshTokenExpiryTime = "";
        await db!.UpdateAsync(user);

        await db.DeleteAllAsync<UserInstance>();
    }


    public async Task<UserInstance> GetUserInstanceAsync()
    {
        await Init();
        return await db!.Table<UserInstance>().FirstOrDefaultAsync();
    }


    public async Task<User> GetUserAsync(int id)
    {
        await Init();
        return await db!.Table<User>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }


    public async Task UpdateUserAsync(User updatedUser)
    {
        await Init();
        await db!.UpdateAsync(updatedUser);
    }


    public async Task<List<User>> GetEmployees(int businessId)
    {
        await Init();
        return await db!.Table<User>()
                        .Where(x => x.BusinessRefId == businessId)
                        .ToListAsync();
    }
}
