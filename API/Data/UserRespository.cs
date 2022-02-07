using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRespository : IUserRespository
    {
         public DataContext _context { get; }
         public IMapper _mapper{get;set;}
        public UserRespository(DataContext context,IMapper mapper)
        {
            _context=context;
            _mapper=mapper;
        }
        public async Task<UserApp> GetUserByIDAsync(int ID)
        {
            return await _context.Users.FindAsync(ID);
        }

        public async Task<UserApp> GetUserByUserNameAsync(string name)
        {
            return await _context.Users.Include(p=>p.Photos).SingleOrDefaultAsync(x=>x.userName==name);
        }

        public async Task<IEnumerable<UserApp>> GetUsersAsync()
        { 
            return await _context.Users.Include(p=>p.Photos)
            .ToListAsync();
        }

        // public async Task<bool> SaveAllAsync()
        // {
        //     return await _context.SaveChangesAsync()>0;
        // }

        public void Update(UserApp user)
        {
            _context.Entry(user).State=EntityState.Modified;
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userparams)
        {
            var query= _context.Users.AsQueryable();
            var minDob=DateTime.Today.AddYears(-userparams.minAge-1);
            var maxDob=DateTime.Today.AddYears(-userparams.maxAge);
            query=query.Where(x=>x.userName!=userparams.currentUserName 
                              && x.Gender==userparams.gender
                              );
            query=query.Where(x=>x.DateOfBirth >= maxDob && x.DateOfBirth<=minDob); // ****** error
            query=userparams.orderBy switch 
            {
               "created"=> query.OrderByDescending(u=>u.Created),
               _ =>query.OrderByDescending(u=>u.LastActive)
                
            };
            
            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking()
            ,userparams.pageNumber,userparams.pageSize);                          
                                        
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            
            return await _context.Users.Where(x=>x.userName==username)
                                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                                        .FirstOrDefaultAsync();
        }
    }
}