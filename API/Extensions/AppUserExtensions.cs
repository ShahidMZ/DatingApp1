using System;
using API.DTOs;
using API.Entities;
using API.Services.Interfaces;

namespace API.Extensions;

public static class AppUserExtensions
{
    public static UserDTO ToDTO(this AppUser user, ITokenService tokenService)
    {
        return new UserDTO
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }
}
