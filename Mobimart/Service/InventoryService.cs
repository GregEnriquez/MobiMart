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
        var businessId = Guid.Empty;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        return await db!.Table<Item>().Where(x => x.BusinessId == businessId).ToListAsync();
    }


    public async Task<Description> GetItemDescAsync(string barcode)
    {
        await Init();

        var item = await GetItemAsync(barcode);
        if (item == null) return null;

        return await db!.Table<Description>().FirstOrDefaultAsync(x => x.ItemId == item.Id);
    }


    public async Task AddItemAsync(Item i)
    {
        await Init();
        i.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.InsertAsync(i);
    }


    public async Task AddDescAsync(Description x)
    {
        await Init();
        x.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.InsertAsync(x);
    }


    public async Task AddDeliveryAsync(Delivery x)
    {
        await Init();
        x.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.InsertAsync(x);
    }


    public async Task AddInventoryAsync(Inventory x)
    {
        await Init();
        x.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.InsertAsync(x);
    }



    public async Task<List<Inventory>> GetInventoriesAsync()
    {
        await Init();
        var businessId = Guid.Empty;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        return await db!.Table<Inventory>().Where(x => x.BusinessId == businessId).ToListAsync();
    }



    public async Task<Inventory> GetInventoryAsync(Guid id)
    {
        await Init();
        return await db!.Table<Inventory>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }



    public async Task<List<Inventory>> GetInventoriesAsync(string barcode)
    {
        await Init();
        var businessId = Guid.Empty;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        return await db!.Table<Inventory>().Where(x => x.BusinessId == businessId && x.ItemBarcode == barcode).ToListAsync();
    }


    public async Task<List<Delivery>> GetDeliveriesAsync(string barcode)
    {
        await Init();
        var businessId = Guid.Empty;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        return await db!.Table<Delivery>().Where(x => x.BusinessId == businessId && x.ItemBarcode == barcode).ToListAsync();
    }


    public async Task<List<Delivery>> GetDeliveriesViaDate(DateTime date)
    {
        await Init();
        var businessId = Guid.Empty;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }

        var allDeliveries = await db!.Table<Delivery>().Where(x => x.BusinessId == businessId).ToListAsync();

        return allDeliveries.Where(x => x.DateDelivered.LocalDateTime.Date == date.Date).ToList();
    }


    public async Task<List<Delivery>> GetDeliveriesViaSupplier(Guid supplierId)
    {
        await Init();
        var businessId = Guid.Empty;
        if (Shell.Current.BindingContext is FlyoutMenuViewModel vm)
        {
            businessId = vm.BusinessId;
        }
        return await db!.Table<Delivery>().Where(x => x.BusinessId == businessId && x.SupplierId == supplierId).ToListAsync();
    }


    public async Task<List<Delivery>> GetDeliveriesViaItem(string barcode)
    {
        await Init();
        var businessId = Guid.Empty;
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

        Inventory? inv = null;
        foreach (var d in deliveries)
        {
            inv = await GetInventoryFromDeliveryAsync(d.Id);

            output.Add(new DeliveryRecord()
            {
                DeliveryId = d.Id,
                ItemName = item.Name,
                DelivQuantity = d.DeliveryAmount,
                DateDelivered = d.DateDelivered.LocalDateTime,
                DateExpire = d.ExpirationDate.LocalDateTime,
                BatchCostPrice = (float)d.BatchWorth,
                ItemType = item.Type,
                ItemDesc = desc?.Text ?? "",
                Barcode = item.Barcode,
                QuantityInStock = inv is null || inv.IsDeleted ? 0 : inv.TotalAmount,
                ConsignmentSchedule = d.ConsignmentSchedule,
                ReturnByDate = d.ReturnByDate is null ? null : d.ReturnByDate.Value.LocalDateTime
            });
        }

        return output;
    }


    public async Task<List<DeliveryRecord>> GetDeliveryRecordsAsync(Guid supplierId)
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
                DateDelivered = d.DateDelivered.LocalDateTime,
                DateExpire = d.ExpirationDate.LocalDateTime,
                BatchCostPrice = (double)d.BatchWorth,
                ItemType = item.Type,
                ItemDesc = desc!.Text,
                Barcode = item.Barcode,
                QuantityInStock = inv is not null ? inv.TotalAmount : 0,
                ConsignmentSchedule = d.ConsignmentSchedule,
                // ReturnByDate =  d.ReturnByDate!.Value.LocalDateTime.ToString() ?? ""
                ReturnByDate =  d.ReturnByDate is null ? null : d.ReturnByDate!.Value.LocalDateTime
            });
        }


        return output;
    }


    public async Task<Delivery> GetDeliveryAsync(Guid id)
    {
        await Init();
        return await db!.Table<Delivery>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }



    public async Task<Inventory> GetInventoryFromDeliveryAsync(Guid deliveryId)
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
                Price = (float)item.RetailPrice,
                Type = item.Type,
                Description = desc?.Text ?? ""
            });
        }

        return output;
    }


    public async Task UpdateDescAsync(Description desc)
    {
        await Init();
        desc.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.UpdateAsync(desc);
    }



    public async Task UpdateItemAsync(Item item)
    {
        await Init();
        item.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.UpdateAsync(item);
    }



    public async Task UpdateDeliveryAsync(Delivery x)
    {
        await Init();
        x.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.UpdateAsync(x);
    }


    public async Task UpdateInventoryAsync(Inventory x)
    {
        await Init();
        x.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.UpdateAsync(x);
    }


    public async Task DeleteInventory(Inventory x)
    {
        await Init();
        // soft delete
        x.IsDeleted = true;
        x.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.UpdateAsync(x);
        // instead of actual delete
        // await db!.DeleteAsync(x);
    }


    public async Task DeleteDelivery(Delivery x)
    {
        await Init();
        // soft delete
        x.IsDeleted = true;
        x.LastUpdatedAt = DateTimeOffset.UtcNow;
        await db!.UpdateAsync(x);
        // instead of actual delete
        // await db!.DeleteAsync(x);
    }


    public async Task<SupplierContract?> GetSupplierContractAsync(Supplier supplier, DateTime returnDate)
    {
        await Init();

        var itemsDesc = await GetAllItemsAsync();
        var deliveries = await GetDeliveriesViaSupplier(supplier.Id);

        if (deliveries is null) return null;

        // filter by date
        var queriedDeliveries = deliveries.Where(x => !(x.ReturnByDate == null) && x.ReturnByDate.Value.Date == returnDate.Date).AsEnumerable();
        if (queriedDeliveries is null || !queriedDeliveries.Any()) return null;
        // if (deliveries.Count > 0) deliveries = [.. queriedDeliveries];
        deliveries = [.. queriedDeliveries];

        var items = new List<ContractItem>();
        decimal total = 0;

        for (int i = 0; i < deliveries!.Count; i++)
        {
            var delivery = deliveries[i];
            // if (delivery.ReturnByDate is null || delivery.ReturnByDate.Equals("")) continue;
            // if (DateTime.Parse(delivery.ReturnByDate).Date != returnDate.Date) continue;

            var inv = await GetInventoryFromDeliveryAsync(delivery.Id);
            var itemDesc = itemsDesc.Find(x => x.Barcode.Equals(delivery.ItemBarcode));
            string name = itemDesc?.Name ?? "Unknown";

            decimal unitPrice = delivery.BatchWorth / delivery.DeliveryAmount;

            int soldQuantity = inv is null || inv.IsDeleted ? delivery.DeliveryAmount : delivery.DeliveryAmount - inv.TotalAmount;
            int returnQuantity = delivery.DeliveryAmount - soldQuantity;

            total += soldQuantity * unitPrice;

            if (items.Find(x => x.Name.Equals(name)) is ContractItem item)
            {
                item.SoldQuantity += soldQuantity;
                item.ReturnQuantity += returnQuantity;
                continue;
            }
            items.Add(new()
            {
                Name = name,
                SoldQuantity = soldQuantity,
                ReturnQuantity = returnQuantity
            });
        }

        if (items.Count <= 0) return null;

        return new SupplierContract()
        {
            SupplierId = supplier.Id,
            SupplierName = supplier.Name,
            ReturnDate = returnDate,
            IsDropped = false,
            Items = [.. items],
            AmountToPay = total
        };
    }


    public async Task RemoveCongsignmentInventoryViaReturnDate(Supplier supplier, DateTime returnDate)
    {
        await Init();

        var deliveries = await GetDeliveriesViaSupplier(supplier.Id);
        if (deliveries.Count <= 0) return;


        deliveries = [.. deliveries.Where(x => x.ReturnByDate is not null && x.ReturnByDate.Value.Date == returnDate.Date).AsEnumerable()];
        for (int i = 0; i < deliveries.Count; i++)
        {
            var delivery = deliveries[i];
            var inv = await GetInventoryFromDeliveryAsync(delivery.Id);

            delivery.ReturnByDate = null;
            await UpdateDeliveryAsync(delivery);
            if (inv is not null)
                await DeleteInventory(inv);
        }
    }
}
