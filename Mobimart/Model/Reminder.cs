using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MobiMart.Model;

public enum ReminderType {SupplyRunout, ConsignmentDue, DeliveryReminder}

public class Reminder
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [ForeignKey(nameof(Business))]
    public int BusinessId { get; set; }
    public ReminderType Type { get; set; }
    public string Title { get; set; } = "";
    public string Message { get; set; } = "";
    public string NotifyAtDate { get; set; } = "";
    public bool RepeatDaily { get; set; } = false;
    public int RelatedEntityId { get; set; } // e.g., InventoryId or DeliveryId
    public bool IsEnabled { get; set; } = false;
    public bool Sent { get; set; } = false;

}
