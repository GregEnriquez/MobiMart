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
            BusinessRefId = newUser.BusinessId,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Email = newUser.Email,
            Password = newUser.Password,
            BirthDate = newUser.BirthDate,
            Age = newUser.Age,
            PhoneNumber = newUser.PhoneNumber,
            EmployeeType = newUser.EmployeeType
        };
    }

    public static User ToEntity(this UpdateUserDto newUser, int id)
    {
        return new User()
        {
            Id = id,
            BusinessRefId = newUser.BusinessId,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Email = newUser.Email,
            Password = newUser.Password,
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
            user.BusinessRefId,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Password,
            user.BirthDate,
            user.Age,
            user.PhoneNumber,
            user.EmployeeType
        );
    }

}
