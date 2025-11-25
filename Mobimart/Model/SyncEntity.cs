using System;
using SQLite;

namespace MobiMart.Model;

public class SyncEntity
{
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset LastUpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public bool IsDeleted { get; set; } = false;
}
