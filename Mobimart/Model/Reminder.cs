using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public enum ReminderType {SupplyRunout, ConsignmentDue, DeliveryReminder}

public class Reminder : SyncEntity
{
    [Indexed]
    public Guid BusinessId { get; set; }

    public ReminderType Type { get; set; }
    public string Title { get; set; } = "";
    public string Message { get; set; } = "";
    public DateTimeOffset NotifyAtDate { get; set; }
    public bool RepeatDaily { get; set; } = false;
    public Guid RelatedEntityId { get; set; } // e.g., InventoryId or DeliveryId
    public bool IsEnabled { get; set; } = false;
    public bool Sent { get; set; } = false;

}
