using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extension;
using API.Helper;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseAPIController
    {
        public IUnitOfWork _unitOfWork{get;}
       
        public LikesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId=User.GetUserID(); //login user 
            var likedUser=await _unitOfWork.UserRespository.GetUserByUserNameAsync(username); // user who login user want to like
            var sourceUser=await _unitOfWork.likesRespository.GetUserWithLikes(sourceUserId); //user info who want to give like
            if(likedUser==null) return NotFound();
            if(sourceUser.userName == username) return BadRequest("You cannot like yourself");
            var userLike=await _unitOfWork.likesRespository.GetUserLike(sourceUserId,likedUser.ID);
            if(userLike !=null) return BadRequest("You already liked this user");
            userLike=new UserLike{
                SourceUserId=sourceUserId,
                LikedUserId=likedUser.ID
            };
            sourceUser.LikedUsers.Add(userLike);
            if(await _unitOfWork.Complete()) return Ok();
            return BadRequest("Fail to like user");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLike([FromQuery]LikesParams likesParams)
        {
            likesParams.userId=User.GetUserID();
            likesParams.predicate=likesParams.predicate;
            var result=await _unitOfWork.likesRespository.GetUserLikes(likesParams);
            Response.AddPaginationHeader(result.currentPage, result.pageSize,result.totalPages,result.totalCount);
            return Ok(result);
        }
    }
}