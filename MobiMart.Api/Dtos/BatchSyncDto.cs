using System;

namespace MobiMart.Api.Dtos;

// the payload the CLIENT sends to the SERVER (Push)
public record SyncPushDto(
    // Core
    List<CreateBusinessDto>? Businesses,
    List<CreateUserDto>? Users,
    
    // Inventory & Items
    List<CreateItemDto>? Items,
    List<CreateDescriptionDto>? Descriptions,
    List<CreateInventoryDto>? Inventory,
    
    // Supply Chain
    List<CreateSupplierDto>? Suppliers,
    List<CreateDeliveryDto>? Deliveries,
    List<CreateCompletedContractDto>? CompletedContracts,
    List<CreateCompletedContractItemDto>? CompletedContractItems,
    
    // Sales
    List<CreateSalesTransactionDto>? Transactions,
    List<CreateSalesItemDto>? SalesItems,
    
    // Misc
    List<CreateReminderDto>? Reminders,
    List<CreateMonthlyForecastInstanceDto>? Forecasts,

    // Metadata
    DateTimeOffset ClientSyncTimestamp
);

// the payload the SERVER sends to the CLIENT (Pull)
public record SyncPullDto(
    // Core
    List<BusinessDto> Businesses,
    List<UserDto> Users,
    
    // Inventory & Items
    List<ItemDto> Items,
    List<DescriptionDto> Descriptions,
    List<InventoryDto> Inventory,
    
    // Supply Chain
    List<SupplierDto> Suppliers,
    List<DeliveryDto> Deliveries,
    List<CompletedContractDto> CompletedContracts,
    List<CompletedContractItemDto> CompletedContractItems,
    
    // Sales
    List<SalesTransactionDto> Transactions,
    List<SalesItemDto> SalesItems,
    
    // Misc
    List<ReminderDto> Reminders,
    List<MonthlyForecastInstanceDto> Forecasts,

    // Metadata
    DateTimeOffset ServerTimestamp
);
