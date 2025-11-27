using System;
using System.Diagnostics;
using MobiMart.Helpers.Dtos;
using MobiMart.Model;

namespace MobiMart.Helpers.Mapping;

public static class ReminderMapping
{
    public static CreateReminderDto ToDto(this Reminder entity)
    {
        return new CreateReminderDto(
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
            entity.IsDeleted,
            entity.LastUpdatedAt
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
            LastUpdatedAt = dto.LastUpdatedAt,
            IsDeleted = dto.IsDeleted
        };
    }



    public static Reminder ToEntity(this ReminderDto dto)
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
            LastUpdatedAt = dto.LastUpdatedAt,
            IsDeleted = dto.IsDeleted
        };
    }



    public static CreateMonthlyForecastInstanceDto ToDto(this MonthlyForecastInstance entity)
    {
        return new CreateMonthlyForecastInstanceDto(
            entity.Id,
            entity.BusinessId,
            entity.Response,
            entity.DateGenerated,
            entity.IsDeleted,
            entity.LastUpdatedAt
        );
    }


    public static MonthlyForecastInstance ToEntity(this CreateMonthlyForecastInstanceDto dto)
    {
        return new MonthlyForecastInstance
        {
            Id = dto.Id,
            LastUpdatedAt = dto.LastUpdatedAt,
            IsDeleted = dto.IsDeleted,
            BusinessId = dto.BusinessId,
            Response = dto.Response,
            DateGenerated = dto.DateGenerated
        };
    }



    public static MonthlyForecastInstance ToEntity(this MonthlyForecastInstanceDto dto)
    {
        return new MonthlyForecastInstance
        {
            Id = dto.Id,
            LastUpdatedAt = dto.LastUpdatedAt,
            IsDeleted = dto.IsDeleted,
            BusinessId = dto.BusinessId,
            Response = dto.Response,
            DateGenerated = dto.DateGenerated
        };
    }
}
