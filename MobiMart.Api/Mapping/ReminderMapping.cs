using System;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Mapping;

public static class ReminderMapping
{
    public static ReminderDto ToDto(this Reminder entity)
    {
        return new ReminderDto(
            entity.Id,
            entity.BusinessId,
            entity.Type,
            entity.Title,
            entity.Message,
            entity.NotifyAtDate,
            entity.RepeatDaily,
            entity.RelatedEntityId,
            entity.IsEnabled,
            entity.Sent,
            entity.LastUpdatedAt,
            entity.IsDeleted
        );
    }

    public static Reminder ToEntity(this CreateReminderDto dto)
    {
        return new Reminder
        {
            Id = dto.Id,
            BusinessId = dto.BusinessId,
            Type = dto.Type,
            Title = dto.Title,
            Message = dto.Message,
            NotifyAtDate = dto.NotifyAtDate,
            RepeatDaily = dto.RepeatDaily,
            RelatedEntityId = dto.RelatedEntityId,
            IsEnabled = dto.IsEnabled,
            Sent = dto.Sent,
            LastUpdatedAt = DateTimeOffset.UtcNow,
            IsDeleted = dto.IsDeleted
        };
    }

    public static MonthlyForecastInstanceDto ToDto(this MonthlyForecastInstance entity)
    {
        return new MonthlyForecastInstanceDto(
            entity.Id,
            entity.BusinessId,
            entity.Response,
            entity.DateGenerated,
            entity.LastUpdatedAt,
            entity.IsDeleted
        );
    }


    public static MonthlyForecastInstance ToEntity(this CreateMonthlyForecastInstanceDto dto)
    {
        return new MonthlyForecastInstance
        {
            Id = dto.Id,
            LastUpdatedAt = DateTimeOffset.UtcNow,
            IsDeleted = dto.IsDeleted,
            Response = dto.Response,
            DateGenerated = dto.DateGenerated
        };
    }
}
