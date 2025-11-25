using System;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Dtos;

public record ReminderDto(
    Guid Id,
    Guid BusinessId,
    ReminderType Type,
    string Title,
    string Message,
    DateTimeOffset NotifyAtDate,
    bool RepeatDaily,
    Guid RelatedEntityId,
    bool IsEnabled,
    bool Sent,
    DateTimeOffset LastUpdatedAt,
    bool IsDeleted
);

public record CreateReminderDto(
    Guid Id,
    Guid BusinessId,
    ReminderType Type,
    string Title,
    string Message,
    DateTimeOffset NotifyAtDate,
    bool RepeatDaily,
    Guid RelatedEntityId,
    bool IsEnabled,
    bool Sent,
    bool IsDeleted
);

public record MonthlyForecastInstanceDto(
    Guid Id,
    Guid BusinessId,
    string Response,
    DateTimeOffset DateGenerated,
    DateTimeOffset LastUpdatedAt,
    bool IsDeleted
);

public record CreateMonthlyForecastInstanceDto(
    Guid Id,
    Guid BusinessId,
    string Response,
    DateTimeOffset DateGenerated,
    bool IsDeleted
);