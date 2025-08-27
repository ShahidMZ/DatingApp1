using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenService tokenService) : BaseController
{
    [HttpPost("register")]  // POST: api/account/register
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
    {
        if (await EmailExists(registerDto.Email))
        {
            return BadRequest("Email is already in use!");
        }

        using HMACSHA512 hmac = new();

        AppUser user = new()
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user.ToDTO(tokenService);
    }

    [HttpPost("login")] // POST: api/account/login
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
    {
        AppUser? user = await context.Users.SingleOrDefaultAsync(x =>
            x.Email.ToLower() == loginDto.Email.ToLower());

        if (user == null)
        {
            return Unauthorized("Invalid email!");
        }

        using HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < hash.Length; i++)
        {
            if (hash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid Password!");
            }
        }

        return user.ToDTO(tokenService);
    }

    private async Task<bool> EmailExists(string email)
    {
        return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }
}
