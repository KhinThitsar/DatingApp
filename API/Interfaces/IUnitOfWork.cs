using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRespository UserRespository{get;}
        ILikesRespository likesRespository{get;}
        Task<bool> Complete();
        bool HasChanges();
    }
}