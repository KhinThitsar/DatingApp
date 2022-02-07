using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helper;

namespace API.Interfaces
{
    public interface IUserRespository
    {
        void Update(UserApp user);
        //Task<bool> SaveAllAsync();
        Task<IEnumerable<UserApp>> GetUsersAsync();

        Task<UserApp> GetUserByIDAsync(int ID);

        Task<UserApp> GetUserByUserNameAsync(string name);

        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userparams);
        Task<MemberDto> GetMemberAsync(string username);
    }
}