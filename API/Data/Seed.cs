using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using API.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUser(DataContext context)
        {
            if(await context.Users.AnyAsync()) return;

            var userData=await System.IO.File.ReadAllTextAsync("Data/UsersSeedData.json");
            var users=JsonSerializer.Deserialize<List<UserApp>>(userData);
            foreach (var user in users)
            {
                using var hmac=new HMACSHA512();
                user.userName=user.userName.ToLower();
                user.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes("1234"));
                user.PasswordSalt=hmac.Key;
                context.Users.Add(user);
            }
            
            await context.SaveChangesAsync();
        }
    }
}