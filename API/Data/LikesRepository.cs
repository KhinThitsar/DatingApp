using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extension;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using API.Helper;
namespace API.Data
{
    public class LikesRepository : ILikesRespository
    {
         public DataContext _context { get; }
        public LikesRepository(DataContext context)
        {
            _context=context;


        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int likeUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId,likeUserId);
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users=_context.Users.OrderBy(x=>x.userName).AsQueryable();
            var likes=_context.Likes.AsQueryable();
            if(likesParams.predicate=="Liked") // users that currently login user(userid) liked
            {
                likes=likes.Where(x=>x.SourceUserId==likesParams.userId);
                users=likes.Select(x=>x.LikedUser);
            }
            else{ // users that liked currently login user(userid)
                likes=likes.Where(x=>x.LikedUserId==likesParams.userId);
                users=likes.Select(x=>x.SourceUser);
            }
           var likedUsers= users.Select(x=>new LikeDto{
                Id=x.ID,
                userName=x.userName,
                photoUrl=x.Photos.FirstOrDefault(p=>p.IsMain).Url,
                age=x.DateOfBirth.calculateAge(),
                knowAs=x.KnownAs,
                city=x.City
            });
            return await PagedList<LikeDto>.CreateAsync(likedUsers,likesParams.pageNumber,likesParams.pageSize);
        }

        public async Task<UserApp> GetUserWithLikes(int userId) //return a list of users that given userid liked
        {
            return await _context.Users.Include(x=>x.LikedUsers).FirstOrDefaultAsync(x=>x.ID==userId);
        }
    }
}