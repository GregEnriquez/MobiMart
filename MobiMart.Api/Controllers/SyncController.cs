using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobiMart.Api.Data;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;
using MobiMart.Api.Mapping;

namespace MobiMart.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Require Login to Sync
public class SyncController(MobiMartContext db) : ControllerBase
{
    // -- PULL: Get changes from Server to Client --
    [Authorize]
    [HttpGet]
    // Example URL: /api/sync?businessId=...&timestampSince=...
    public async Task<ActionResult<SyncPullDto>> PullChanges([FromQuery] Guid businessId, [FromQuery] DateTimeOffset? timestampSince)
    {
        // if "timeStampSince" is null (first sync), use MinValue to fetch everything.
        var timestamp = timestampSince ?? DateTimeOffset.MinValue;

        // verify if user actually belongs in the business

        // fetch only records modified AFTER the client's last sync
        // AsNoTracking() makes reads faster since we aren't updating them here.
        
        var businesses = (await db.Businesses.AsNoTracking().ToListAsync())
            .Where(x => x.LastUpdatedAt > timestamp);

        // if no businessId is given it just means the client who requested
        // is currently not affiliated with any business yet (client is a new user)
        if (businessId == Guid.Empty)
        {
            var businessOnlyResponse = new SyncPullDto(
                Businesses: businesses.Select(x => x.ToDto()).ToList(),
                Users: [],
                Items: [],
                Descriptions: [],
                Inventory: [],
                Suppliers: [],
                Deliveries: [],
                CompletedContracts: [],
                CompletedContractItems: [],
                Transactions: [],
                SalesItems: [],
                Reminders: [],
                Forecasts: [],
                // Critical: Tell the client the exact time of this snapshot
                ServerTimestamp: DateTimeOffset.UtcNow 
            );

            return Ok(businessOnlyResponse);
        }

        var users = (await db.Users.AsNoTracking()
            .Where(x => x.BusinessId == businessId).ToListAsync())
            .Where(x => x.LastUpdatedAt >= timestamp).ToList();

        var items = (await db.Items.AsNoTracking()
            .Where(x => x.BusinessId == businessId).ToListAsync())
            .Where(x => x.LastUpdatedAt >= timestamp).ToList();
        
        // JOIN LOGIC (expensive on cpu)
        var descriptions = (await db.Descriptions.AsNoTracking()
            .Where(x => db.Items.Any(i => i.Id == x.ItemId && i.BusinessId == businessId)).ToListAsync())
            .Where(x => x.LastUpdatedAt >= timestamp).ToList();

        var inventories = (await db.Inventories.AsNoTracking()
            .Where(x => x.BusinessId == businessId).ToListAsync())
            .Where(x => x.LastUpdatedAt >= timestamp).ToList();

        var suppliers = (await db.Suppliers.AsNoTracking()
            .Where(x => x.BusinessId == businessId).ToListAsync())
            .Where(x => x.LastUpdatedAt >= timestamp).ToList();

        var deliveries = (await db.Deliveries.AsNoTracking()
            .Where(x => x.BusinessId == businessId).ToListAsync())
            .Where(x => x.LastUpdatedAt >= timestamp).ToList();

        var transactions = (await db.SalesTransactions.AsNoTracking()
            .Where(x => x.BusinessId == businessId).ToListAsync())
            .Where(x => x.LastUpdatedAt >= timestamp).ToList();

        var salesItems = (await db.SalesItems.AsNoTracking()
            .Where(x => db.SalesTransactions.Any(t => t.Id == x.TransactionId && t.BusinessId == businessId)).ToListAsync())
            .Where(x => x.LastUpdatedAt >= timestamp).ToList();

        var reminders = (await db.Reminders.AsNoTracking()
            .Where(x => x.BusinessId == businessId).ToListAsync())
            .Where(x => x.LastUpdatedAt >= timestamp).ToList();

        var completedContracts = (await db.CompletedContracts.AsNoTracking()
            .Where(x => x.BusinessId == businessId).ToListAsync())
            .Where(x => x.LastUpdatedAt >= timestamp).ToList();
        
        var completedContractItems = (await db.CompletedContractItems.AsNoTracking()
            .Where(x => db.CompletedContracts.Any(c => c.Id == x.ContractId && c.BusinessId == businessId)).ToListAsync())
            .Where(x => x.LastUpdatedAt >= timestamp).ToList();

        var forecasts = (await db.Forecasts.AsNoTracking()
            .Where(x => x.BusinessId == businessId).ToListAsync())
            .Where(x => x.LastUpdatedAt >= timestamp).ToList();
        

        var response = new SyncPullDto(
            Businesses: businesses.Select(x => x.ToDto()).ToList(),
            Users: users.Select(x => x.ToDto()).ToList(),
            Items: items.Select(x => x.ToDto()).ToList(),
            Descriptions: descriptions.Select(x => x.ToDto()).ToList(),
            Inventory: inventories.Select(x => x.ToDto()).ToList(),

            Suppliers: suppliers.Select(x => x.ToDto()).ToList(),
            Deliveries: deliveries.Select(x => x.ToDto()).ToList(),
            CompletedContracts: completedContracts.Select(x => x.ToDto()).ToList(), 
            CompletedContractItems: completedContractItems.Select(x => x.ToDto()).ToList(),

            Transactions: transactions.Select(x => x.ToDto()).ToList(),
            SalesItems: salesItems.Select(x => x.ToDto()).ToList(),
            Reminders: reminders.Select(x => x.ToDto()).ToList(),
            Forecasts: forecasts.Select(x => x.ToDto()).ToList(), // Add logic
            
            // Critical: Tell the client the exact time of this snapshot
            ServerTimestamp: DateTimeOffset.UtcNow 
        );

        return Ok(response);
    }

    // -- PUSH: Client sends changes to Server --
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PushChanges([FromBody] SyncPushDto payload)
    {
        // 1. Start a Transaction. 
        // If saving "SalesItem" fails, we want to roll back "SalesTransaction" too.
        using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            // -- Core --
            if (payload.Businesses?.Any() == true)
                await UpsertBatch(payload.Businesses, db.Businesses, (dto) => dto.ToEntity());
            
            if (payload.Users?.Any() == true)
                await UpsertBatch(payload.Users, db.Users, (dto) => dto.ToEntity());

            // -- Inventory & Items --
            if (payload.Items?.Any() == true)
                await UpsertBatch(payload.Items, db.Items, (dto) => dto.ToEntity());

            if (payload.Descriptions?.Any() == true)
                await UpsertBatch(payload.Descriptions, db.Descriptions, (dto) => dto.ToEntity());

            if (payload.Inventory?.Any() == true)
                await UpsertBatch(payload.Inventory, db.Inventories, (dto) => dto.ToEntity());

            // -- Supply Chain --
            if (payload.Suppliers?.Any() == true)
                await UpsertBatch(payload.Suppliers, db.Suppliers, (dto) => dto.ToEntity());

            if (payload.Deliveries?.Any() == true)
                await UpsertBatch(payload.Deliveries, db.Deliveries, (dto) => dto.ToEntity());

            if (payload.CompletedContracts?.Any() == true)
                await UpsertBatch(payload.CompletedContracts, db.CompletedContracts, (dto) => dto.ToEntity());

            if (payload.CompletedContractItems?.Any() == true)
                await UpsertBatch(payload.CompletedContractItems, db.CompletedContractItems, (dto) => dto.ToEntity());


            // -- Sales --
            if (payload.Transactions?.Any() == true)
                await UpsertBatch(payload.Transactions, db.SalesTransactions, (dto) => dto.ToEntity());

            if (payload.SalesItems?.Any() == true)
                await UpsertBatch(payload.SalesItems, db.SalesItems, (dto) => dto.ToEntity());

            // -- Misc --
            if (payload.Reminders?.Any() == true)
                await UpsertBatch(payload.Reminders, db.Reminders, (dto) => dto.ToEntity());

            if (payload.Forecasts?.Any() == true)
                await UpsertBatch(payload.Forecasts, db.Forecasts, (dto) => dto.ToEntity());
            

            // Save everything to DB
            await db.SaveChangesAsync();
            
            // Commit the transaction
            await transaction.CommitAsync();

            return Ok(new { Message = "Sync Successful", ServerTime = DateTimeOffset.UtcNow });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            // Log error (Console or Logger)
            Console.WriteLine($"Sync Error: {ex.Message}");
            return StatusCode(500, $"Sync failed on server.\n\nError:\n{ex.Message}");
        }
    }

    // HELPER: generic upsert logic
    private async Task UpsertBatch<TEntity, TDto>(
        List<TDto> dtos,
        DbSet<TEntity> dbSet,
        Func<TDto, TEntity> mapper) where TEntity : SyncEntity
    {
        // 1. Get all IDs from the incoming payload
        //    (Assumes your DTOs have an 'Id' property - you might need a common interface for DTOs or just reflection/dynamic)
        //    Since DTOs are records and don't share a base, we can't easily map .Id without an interface.
        //    However, since we convert to Entity first, we can use the Entity ID.
        
        var incomingEntities = dtos.Select(mapper).ToList();
        var incomingIds = incomingEntities.Select(e => e.Id).ToList();

        // 2. BULK READ: Load ALL existing entities from DB in ONE query
        var existingEntitiesDict = await dbSet
            .Where(e => incomingIds.Contains(e.Id))
            .ToDictionaryAsync(e => e.Id);

        // 3. Loop in Memory (Fast!)
        foreach (var incoming in incomingEntities)
        {
            if (existingEntitiesDict.TryGetValue(incoming.Id, out var existing))
            {
                // UPDATE
                if (incoming.LastUpdatedAt > existing.LastUpdatedAt)
                {
                    db.Entry(existing).CurrentValues.SetValues(incoming);
                }
            }
            else
            {
                // INSERT
                await dbSet.AddAsync(incoming);
            }
        }
    }
}