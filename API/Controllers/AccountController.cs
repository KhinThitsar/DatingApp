using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController :BaseAPIController
    {
        public DataContext _context { get; }
        public ITokenService _tokenService { get; }
        public IMapper _mapper { get; }
        public AccountController(DataContext context,ITokenService tokenService,IMapper mapper) 
        {
            this._tokenService = tokenService;
            this._context = context;
            this._mapper=mapper;
   
        } 
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto logindata)
        {
            var user=await _context.Users.Include(p=>p.Photos).SingleOrDefaultAsync(
                x=>x.userName==logindata.userName
            );
            if(user==null) return Unauthorized("Invalid username");
            using var hmac=new HMACSHA512(user.PasswordSalt); //need to pass the login user password salt
            var computedHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(logindata.password)); // hash value of login pw
            for(int i=0;i<computedHash.Length;i++)
            {
                //if login hash value != db hash value return
                if(computedHash[i]!=user.PasswordHash[i]) return Unauthorized("Password is wrong");
            }
            return new UserDto{
                userName=user.userName,
                token=_tokenService.CreateToken(user),
                photoUrl=user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                knowAs=user.KnownAs,
                gender=user.Gender
            };
        } 
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerdata)
        {
            if(await UserExist(registerdata.userName)) return BadRequest("User Name Taken");
            var user=_mapper.Map<UserApp>(registerdata);
            using var hmac=new HMACSHA512();
            user.userName=registerdata.userName.ToLower();
            user.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerdata.password));
            user.PasswordSalt=hmac.Key;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto{
                userName=user.userName,
                token=_tokenService.CreateToken(user),
                knowAs=user.KnownAs,
                gender=user.Gender
            };
        }
        private async Task<bool> UserExist(string userName)
        {
            var result=true;
            if(userName!=null)
            {
                result=await _context.Users.AnyAsync(x=>x.userName== userName.ToLower());
            }
            return  result;
        }
    }
    
}