using System;
using MobiMart.Model;
using MobiMart.ViewModel;
using SQLite;

namespace MobiMart.Service;

public class InventoryService
{
    static SQLiteAsyncConnection? db;
    string baseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5199" : "http://localhost:5198";
    HttpClient client;

    public InventoryService()
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

        await db.CreateTableAsync<Inventory>();
        await db.CreateTableAsync<Delivery>();
        await db.CreateTableAsync<Item>();
        await db.CreateTableAsync<Description>();
    }


    public async Task<Item> GetItemAsync(string barcode)
    {
        await Init();
        return await db!.Table<Item>().FirstOrDefaultAsync(x => x.Barcode.Equals(barcode));
    }


    public async Task<List<Item>> GetAllItemsAsync()
    {
        await Init();
        var businessId = -1;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        return await db!.Table<Item>().Where(x => x.BusinessId == businessId).ToListAsync();
    }


    public async Task<Description> GetItemDescAsync(string barcode)
    {
        await Init();
        return await db!.Table<Description>().FirstOrDefaultAsync(x => x.ItemId.Equals(barcode));
    }


    public async Task AddItemAsync(Item i)
    {
        await Init();
        await db!.InsertAsync(i);
    }


    public async Task AddDescAsync(Description x)
    {
        await Init();
        await db!.InsertAsync(x);
    }


    public async Task AddDeliveryAsync(Delivery x)
    {
        await Init();
        await db!.InsertAsync(x);
    }


    public async Task AddInventoryAsync(Inventory x)
    {
        await Init();
        await db!.InsertAsync(x);
    }



    public async Task<List<Inventory>> GetInventoriesAsync()
    {
        await Init();
        var businessId = -1;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        return await db!.Table<Inventory>().Where(x => x.BusinessId == businessId).ToListAsync();
    }



    public async Task<List<Inventory>> GetInventoriesAsync(string barcode)
    {
        await Init();
        var businessId = -1;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        return await db!.Table<Inventory>().Where(x => x.BusinessId == businessId && x.ItemBarcode == barcode).ToListAsync();
    }


    public async Task<List<Delivery>> GetDeliveriesAsync(string barcode)
    {
        await Init();
        var businessId = -1;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        return await db!.Table<Delivery>().Where(x => x.BusinessId == businessId && x.ItemBarcode == barcode).ToListAsync();
    }


    public async Task<List<Delivery>> GetDeliveriesViaSupplier(int supplierId)
    {
        await Init();
        var businessId = -1;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        return await db!.Table<Delivery>().Where(x => x.BusinessId == businessId && x.SupplierId == supplierId).ToListAsync();
    }


    public async Task<List<Delivery>> GetDeliveriesViaItem(string barcode)
    {
        await Init();
        var businessId = -1;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        return await db!.Table<Delivery>().Where(x => x.BusinessId == businessId && x.ItemBarcode.Equals(barcode)).ToListAsync();
    }


    public async Task<List<DeliveryRecord>> GetDeliveryRecordsViaItem(string barcode)
    {
        await Init();
        var output = new List<DeliveryRecord>();
        var deliveries = await GetDeliveriesViaItem(barcode);
        var item = await GetItemAsync(barcode);
        var desc = await GetItemDescAsync(barcode);
        Inventory inv = null;
        foreach (var d in deliveries)
        {
            inv = await GetInventoryFromDeliveryAsync(d.Id);

            output.Add(new DeliveryRecord()
            {
                DeliveryId = d.Id,
                ItemName = item.Name,
                DelivQuantity = d.DeliveryAmount,
                DateDelivered = d.DateDelivered,
                DateExpire = d.ExpirationDate,
                BatchCostPrice = d.BatchWorth,
                ItemType = item.Type,
                ItemDesc = desc!.Text,
                Barcode = item.Barcode,
                QuantityInStock = inv!.TotalAmount,
                ConsignmentSchedule = d.ConsignmentSchedule,
                ReturnByDate = d.ReturnByDate
            });
        }

        return output;
    }


    public async Task<List<DeliveryRecord>> GetDeliveryRecordsAsync(int supplierId)
    {
        await Init();
        var output = new List<DeliveryRecord>();
        var deliveries = await GetDeliveriesViaSupplier(supplierId);
        Item item = null;
        Description desc = null;
        Inventory inv = null;
        foreach (var d in deliveries)
        {
            if (item is null || !item.Barcode.Equals(d.ItemBarcode))
            {
                item = await GetItemAsync(d.ItemBarcode);
                desc = await GetItemDescAsync(item.Barcode);
            }
            inv = await GetInventoryFromDeliveryAsync(d.Id);

            output.Add(new DeliveryRecord()
            {
                DeliveryId = d.Id,
                ItemName = item.Name,
                DelivQuantity = d.DeliveryAmount,
                DateDelivered = d.DateDelivered,
                DateExpire = d.ExpirationDate,
                BatchCostPrice = d.BatchWorth,
                ItemType = item.Type,
                ItemDesc = desc!.Text,
                Barcode = item.Barcode,
                QuantityInStock = inv!.TotalAmount,
                ConsignmentSchedule = d.ConsignmentSchedule,
                ReturnByDate = d.ReturnByDate
            });
        }


        return output;
    }


    public async Task<Delivery> GetDeliveryAsync(int id)
    {
        await Init();
        return await db!.Table<Delivery>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }



    public async Task<Inventory> GetInventoryFromDeliveryAsync(int deliveryId)
    {
        await Init();
        return await db!.Table<Inventory>().Where(x => x.DeliveryId == deliveryId).FirstOrDefaultAsync();
    }



    public async Task<List<InventoryRecord>> GetInventoryRecordsAsync()
    {
        await Init();
        var output = new List<InventoryRecord>();
        var inventoryList = await GetInventoriesAsync();

        foreach (var i in inventoryList)
        {
            var existingRecord = output.FirstOrDefault(x => x.Barcode == i.ItemBarcode);

            if (existingRecord is not null)
            {
                var outputRecord = output.FirstOrDefault(x => x.Barcode == i.ItemBarcode);
                outputRecord!.Quantity += i.TotalAmount;
                continue;
            }

            var item = await GetItemAsync(i.ItemBarcode);
            var desc = await GetItemDescAsync(i.ItemBarcode);

            output.Add(new InventoryRecord()
            {
                Barcode = i.ItemBarcode,
                Name = item.Name,
                Quantity = i.TotalAmount,
                Price = item.RetailPrice,
                Type = item.Type,
                Description = desc.Text
            });
        }

        return output;
    }


    public async Task UpdateDescAsync(Description desc)
    {
        await Init();
        await db!.UpdateAsync(desc);
    }



    public async Task UpdateItemAsync(Item item)
    {
        await Init();
        await db!.UpdateAsync(item);
    }



    public async Task UpdateDeliveryAsync(Delivery x)
    {
        await Init();
        await db!.UpdateAsync(x);
    }


    public async Task UpdateInventoryAsync(Inventory x)
    {
        await Init();
        await db!.UpdateAsync(x);
    }


    public async Task DeleteInventory(Inventory x)
    {
        await Init();
        await db!.DeleteAsync(x);
    }


    public async Task DeleteDelivery(Delivery x)
    {
        await Init();
        await db!.DeleteAsync(x);
    }
}
