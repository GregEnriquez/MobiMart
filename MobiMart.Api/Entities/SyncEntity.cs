using System;
using System.ComponentModel.DataAnnotations;

namespace MobiMart.Api.Entities;

public class SyncEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset LastUpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public bool IsDeleted { get; set; } = false;
}
