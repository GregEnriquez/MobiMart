using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MobiMart.Helpers.Dtos; // Your DTOs
using MobiMart.Helpers.Mapping; // Your Extension Methods (ToCreateDto, ToEntity)
using MobiMart.Model;    // Your Local Entities
using SQLite;

namespace MobiMart.Service;

public class SyncService
{
    private static SQLiteAsyncConnection? _db;
    private string _baseUrl;
    private readonly HttpClient _client;
    private const string LastSyncKey = "LastSyncTimestamp";

    public SyncService()
    {
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            if (DeviceInfo.DeviceType == DeviceType.Virtual)
                _baseUrl = "http://10.0.2.2:5199"; // Emulator
            else
                _baseUrl = "https://app-mobimart-dev-southeastasia-01.azurewebsites.net"; // Prod
        }
        else
        {
            _baseUrl = "http://localhost:5199"; // Windows
        }

        _client = new HttpClient
        {
            BaseAddress = new Uri(_baseUrl),
            Timeout = TimeSpan.FromSeconds(60) // Increased timeout for large batches
        };
    }

    private async Task Init()
    {
        if (_db != null) return;
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "mobimart.db");
        _db = new SQLiteAsyncConnection(databasePath);
        
        // incase the tables are not yet created
        // --- Auth & Core ---
        await _db.CreateTableAsync<User>();
        await _db.CreateTableAsync<UserInstance>();
        await _db.CreateTableAsync<Business>();

        // --- Inventory & Items ---
        await _db.CreateTableAsync<Item>();
        await _db.CreateTableAsync<Description>();
        await _db.CreateTableAsync<Inventory>();
        await _db.CreateTableAsync<WholesaleInventory>();
        await _db.CreateTableAsync<ConsignmentInventory>();

        // --- Supply Chain ---
        await _db.CreateTableAsync<Supplier>();
        await _db.CreateTableAsync<Delivery>();
        await _db.CreateTableAsync<CompletedContract>();
        await _db.CreateTableAsync<CompletedContractItem>();

        // --- Sales ---
        await _db.CreateTableAsync<SalesTransaction>();
        await _db.CreateTableAsync<SalesItem>();

        // --- Misc ---
        await _db.CreateTableAsync<Reminder>();
        await _db.CreateTableAsync<MonthlyForecastInstance>();
    }


    // --- GATEKEEPER: CLOCK CHECK ---
    private async Task<bool> IsClockSynced()
    {
        try
        {
            // Lightweight ping to check server time. Using /api/sync as a probe if auth allows HEAD/GET.
            // Ideally, use a public endpoint like /api/health or /api/auth/ping
            var request = new HttpRequestMessage(HttpMethod.Head, "/api/auth/login"); 
            var response = await _client.SendAsync(request);

            if (response.Headers.Date.HasValue)
            {
                var serverTime = response.Headers.Date.Value;
                var deviceTime = DateTimeOffset.UtcNow;
                var diff = Math.Abs((deviceTime - serverTime).TotalMinutes);

                if (diff > 5)
                {
                    await MainThread.InvokeOnMainThreadAsync(async () => 
                    {
                        if (Application.Current?.Windows.Count > 0)
                        {
                            await Application.Current.Windows[0].Page!.DisplayAlert("System Time Error",
                                $"Your device clock is off by {Math.Round(diff)} minutes. Please update your settings to sync.", "OK");
                        }
                    });
                    return false;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Clock check failed: {ex.Message}");
            return false; // Assume offline
        }
    }


    public async Task<bool> SyncUserLocallyAsync()
    {
        await Init();
        if (_db is null) return false;

        // 1. Connectivity Check
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet) return false;

        // 2. Auth Check
        var userInstance = await _db.Table<UserInstance>().FirstOrDefaultAsync();
        if (userInstance == null || string.IsNullOrEmpty(userInstance.AccessToken)) return false;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userInstance.AccessToken);
        
        // 3. api call
        var pullResponse = await _client.GetAsync($"/users/{userInstance.UserId}");

        if (pullResponse.IsSuccessStatusCode)
        {
            var pulledUser = (await pullResponse.Content.ReadFromJsonAsync<UserDto>())!.ToEntity();
            var currentUser = await _db.Table<User>().Where(x => x.Id == userInstance.UserId).FirstOrDefaultAsync();

            if (pulledUser.LastUpdatedAt >= currentUser.LastUpdatedAt)
            {
                currentUser.UpdateFromUserObject(pulledUser);
                await _db.UpdateAsync(currentUser);
                return true;
            }
        }

        Debug.WriteLine($"PULL Failed: {pullResponse.ReasonPhrase}");
        return false;
    }


    // --- MAIN SYNC FUNCTION ---
    public async Task<bool> SyncDataAsync()
    {
        await Init();
        if (_db is null) return false;

        // 1. Connectivity Check
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet) return false;

        // 2. Auth Check
        var userInstance = await _db.Table<UserInstance>().FirstOrDefaultAsync();
        if (userInstance == null || string.IsNullOrEmpty(userInstance.AccessToken)) return false;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userInstance.AccessToken);

        // 3. Clock Check
        if (!await IsClockSynced()) return false;

        // 4. Get Timestamp
        var lastSyncStr = Preferences.Get(LastSyncKey, DateTimeOffset.MinValue.ToString("o"));
        var lastSync = DateTimeOffset.Parse(lastSyncStr);

        try
        {
            // STEP A: GATHER LOCAL CHANGES (PUSH)
            
            // Fetch all modified records locally
            var businesses = await _db.Table<Business>()
                .Where(x => x.LastUpdatedAt > lastSync).ToListAsync();
            var users = await _db.Table<User>()
                .Where(x => x.LastUpdatedAt > lastSync).ToListAsync();
            var items = await _db.Table<Item>()
                .Where(x => x.LastUpdatedAt > lastSync).ToListAsync();
            var descs = await _db.Table<Description>()
                .Where(x => x.LastUpdatedAt > lastSync).ToListAsync();
            var inventory = await _db.Table<Inventory>().
                Where(x => x.LastUpdatedAt > lastSync).ToListAsync();
            
            var suppliers = await _db.Table<Supplier>()
                .Where(x => x.LastUpdatedAt > lastSync).ToListAsync();
            var deliveries = await _db.Table<Delivery>()
                .Where(x => x.LastUpdatedAt > lastSync).ToListAsync();
            var contracts = await _db.Table<CompletedContract>()
                .Where(x => x.LastUpdatedAt > lastSync).ToListAsync();
            var contractItems = await _db.Table<CompletedContractItem>()
                .Where(x => x.LastUpdatedAt > lastSync).ToListAsync();
            
            var transactions = await _db.Table<SalesTransaction>()
                .Where(x => x.LastUpdatedAt > lastSync).ToListAsync();
            var salesItems = await _db.Table<SalesItem>()
                .Where(x => x.LastUpdatedAt > lastSync).ToListAsync();
            
            var reminders = await _db.Table<Reminder>()
                .Where(x => x.LastUpdatedAt > lastSync).ToListAsync();
            var forecasts = await _db.Table<MonthlyForecastInstance>()
                .Where(x => x.LastUpdatedAt > lastSync).ToListAsync();

            // Check if we have anything to send
            bool hasChanges = businesses.Any() || users.Any() || items.Any() || descs.Any() || inventory.Any() ||
                            suppliers.Any() || deliveries.Any() || contracts.Any() || contractItems.Any() ||
                            transactions.Any() || salesItems.Any() || reminders.Any() || forecasts.Any();

            if (hasChanges)
            {
                // Pack into DTO
                var pushPayload = new SyncPushDto(
                    Businesses: businesses.Select(x => x.ToDto()).ToList(),
                    Users: users.Select(x => x.ToDto()).ToList(),
                    Items: items.Select(x => x.ToDto()).ToList(),
                    Descriptions: descs.Select(x => x.ToDto()).ToList(),
                    Inventory: inventory.Select(x => x.ToDto()).ToList(),
                    
                    Suppliers: suppliers.Select(x => x.ToDto()).ToList(),
                    Deliveries: deliveries.Select(x => x.ToDto()).ToList(),
                    CompletedContracts: contracts.Select(x => x.ToDto()).ToList(),
                    CompletedContractItems: contractItems.Select(x => x.ToDto()).ToList(),
                    
                    Transactions: transactions.Select(x => x.ToDto()).ToList(),
                    SalesItems: salesItems.Select(x => x.ToDto()).ToList(),
                    
                    Reminders: reminders.Select(x => x.ToDto()).ToList(),
                    Forecasts: forecasts.Select(x => x.ToDto()).ToList(),

                    ClientSyncTimestamp: DateTimeOffset.UtcNow
                );

                var pushResponse = await _client.PostAsJsonAsync("/api/sync", pushPayload);
                
                if (!pushResponse.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"PUSH Failed: {pushResponse.ReasonPhrase}");
                    return false; // Abort if push fails to prevent data loss/sync loop
                }
            }

            // STEP B: GET SERVER CHANGES (PULL)

            var currentUser = await _db.Table<User>().Where(x => x.Id == userInstance.UserId).FirstOrDefaultAsync();
            if (currentUser == null) return false;

            // Construct URL with Query Params
            var pullUrl = $"/api/sync?businessId={currentUser.BusinessId}&timestampSince={Uri.EscapeDataString(lastSync.ToString("o"))}";
            
            var pullResponse = await _client.GetAsync(pullUrl);

            if (pullResponse.IsSuccessStatusCode)
            {
                var pullData = await pullResponse.Content.ReadFromJsonAsync<SyncPullDto>();

                if (pullData != null)
                {
                    // STEP C: UNPACK & SAVE (Transactional)
                    
                    await _db.RunInTransactionAsync(tran =>
                    {
                        // Save all lists received from server
                        UpsertLocalList(tran, pullData.Businesses);
                        UpsertLocalList(tran, pullData.Users);
                        UpsertLocalList(tran, pullData.Items);
                        UpsertLocalList(tran, pullData.Descriptions);
                        UpsertLocalList(tran, pullData.Inventory);
                        
                        UpsertLocalList(tran, pullData.Suppliers);
                        UpsertLocalList(tran, pullData.Deliveries);
                        UpsertLocalList(tran, pullData.CompletedContracts);
                        UpsertLocalList(tran, pullData.CompletedContractItems);
                        
                        UpsertLocalList(tran, pullData.Transactions);
                        UpsertLocalList(tran, pullData.SalesItems);
                        
                        UpsertLocalList(tran, pullData.Reminders);
                        UpsertLocalList(tran, pullData.Forecasts);
                    });

                    // STEP D: UPDATE TIMESTAMP
                    Preferences.Set(LastSyncKey, pullData.ServerTimestamp.ToString("o"));
                }
            }
            else
            {
                Debug.WriteLine($"PULL Failed: {pullResponse.ReasonPhrase}");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"CRITICAL SYNC ERROR: {ex}");
            return false;
        }
    }

    // --- HELPER: Generic List Upserter ---
    private void UpsertLocalList<TDto>(SQLiteConnection tran, List<TDto> dtos)
    {
        if (dtos == null || !dtos.Any()) return;

        foreach (var dto in dtos)
        {
            // Map DTO -> Local Entity using Pattern Matching
            object entity = MapCreateDtoToEntity(dto!)!;
            
            if (entity != null)
            {
                tran.InsertOrReplace(entity);
            }
        }
    }

    // --- MAPPER: DTO -> Local Entity Switch ---
    private object? MapCreateDtoToEntity(object dto)
    {
        return dto switch
        {
            BusinessDto b => b.ToEntity(),
            UserDto u => u.ToEntity(),
            ItemDto i => i.ToEntity(),
            DescriptionDto d => d.ToEntity(),
            InventoryDto inv => inv.ToEntity(),

            SupplierDto s => s.ToEntity(),
            DeliveryDto del => del.ToEntity(),
            CompletedContractDto cc => cc.ToEntity(),
            CompletedContractItemDto cci => cci.ToEntity(),

            SalesTransactionDto st => st.ToEntity(),
            SalesItemDto si => si.ToEntity(),

            ReminderDto r => r.ToEntity(),
            MonthlyForecastInstanceDto f => f.ToEntity(),

            _ => null
        };
    }
}