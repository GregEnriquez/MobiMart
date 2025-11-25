using System;
using MobiMart.Api.Dtos;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Mapping;

public static class UserMapping
{
    public static User ToEntity(this CreateUserDto newUser)
    {
        return new User()
        {
            Id = newUser.Id,
            LastUpdatedAt = newUser.LastUpdatedAt,
            IsDeleted = newUser.IsDeleted,
            BusinessId = newUser.BusinessId,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Email = newUser.Email,
            Password = newUser.Password,
            PasswordLength = newUser.PasswordLength,
            BirthDate = newUser.BirthDate,
            Age = newUser.Age,
            PhoneNumber = newUser.PhoneNumber,
            EmployeeType = newUser.EmployeeType
        };
    }

    public static User ToEntity(this UpdateUserDto newUser, Guid id)
    {
        return new User()
        {
            Id = id,
            LastUpdatedAt = DateTimeOffset.UtcNow,
            BusinessId = newUser.BusinessId,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Email = newUser.Email,
            Password = newUser.Password,
            PasswordLength = newUser.PasswordLength,
            BirthDate = newUser.BirthDate,
            Age = newUser.Age,
            PhoneNumber = newUser.PhoneNumber,
            EmployeeType = newUser.EmployeeType
        };
    }

    public static UserDto ToDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.BusinessId,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Password,
            user.PasswordLength,
            user.BirthDate,
            user.Age,
            user.PhoneNumber,
            user.EmployeeType,
            user.LastUpdatedAt,
            user.IsDeleted
        );
    }

}
