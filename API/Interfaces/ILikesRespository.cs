using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helper;
namespace API.Interfaces
{
    public interface ILikesRespository
    {
        Task<UserLike> GetUserLike(int sourceUserId,int likeUserId);
        Task<UserApp> GetUserWithLikes(int userId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}