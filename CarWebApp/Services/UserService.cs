using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using CarWebApp.Configuration;
using CarWebApp.Data;
using CarWebApp.Entities;
using CarWebApp.Exceptions;
using CarWebApp.Models;
using Microsoft.EntityFrameworkCore;


namespace CarWebApp.Services
{
    public interface IUserService
    {
        public Task<string> GetJWT(LoginModel model);
        public Task RegisterUser(RegisterModel model, int roleId = 2);
        public Task<User> GetUser(ClaimsPrincipal claimPrincipal);
    }

    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IPasswordHasher<User> _passwordHasher;


        public UserService(DatabaseContext context, 
            AuthenticationSettings authenticationSettings, 
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _authenticationSettings = authenticationSettings;
            _passwordHasher = passwordHasher;
        }
        
        public async Task<User> GetUser(ClaimsPrincipal claimPrincipal)
        {
            var claim = claimPrincipal.FindFirst(u => u.Type == ClaimTypes.NameIdentifier);
            if (claim == null)
                return null;

            var userId = int.Parse(claim.Value);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }
        
        public async Task<string> GetJWT(LoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.RoleId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);
            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public async Task RegisterUser(RegisterModel model, int roleId = 2)
        {
            User newUser = new User()
            {
                Username = model.Username,
                RoleId = roleId
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, model.Password);
            newUser.PasswordHash = hashedPassword;
            
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }
    }
}