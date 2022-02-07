
using System.Security.Claims;


using API.DTOs;
using API.Entities;
using API.Extension;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseAPIController
    {
        
        public IUnitOfWork _unitOfWork { get; }
        public IMapper _mapper{get;}
        public IPhotoService _photoService{get;}
        public UsersController(IUnitOfWork unitOfWork,IMapper mapper,IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _mapper=mapper;
            _photoService=photoService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userparams)
        {
            var user = await _unitOfWork.UserRespository.GetUserByUserNameAsync(User.GetUserName());
            userparams.currentUserName=user.userName;

            

            var returnUser= await _unitOfWork.UserRespository.GetMembersAsync(userparams);
            Response.AddPaginationHeader(returnUser.currentPage,returnUser.pageSize,returnUser.totalPages,returnUser.totalCount);
            return Ok(returnUser);
            
        }

        
        [HttpGet ("{username}",Name ="GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _unitOfWork.UserRespository.GetMemberAsync(username);
            
            
        }
        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto data)
        {
            var username=User.GetUserName();
            var user=await _unitOfWork.UserRespository.GetUserByUserNameAsync(username);
            _mapper.Map(data,user); // map user update data to user object
            _unitOfWork.UserRespository.Update(user);
            if(await _unitOfWork.Complete()) return NoContent();
            return BadRequest("Fail to update user data");
        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var username=User.GetUserName();
            var user=await _unitOfWork.UserRespository.GetUserByUserNameAsync(username);
            var result=await _photoService.AddPhotoAsync(file);
            if(result.Error!=null) return BadRequest(result.Error.Message);
            var photo=new Photo{
                Url=result.SecureUrl.AbsoluteUri,
                PublicId=result.PublicId
            };
            if(user.Photos.Count==0)
            {
                photo.IsMain=true;
            }
            user.Photos.Add(photo);
            if(await _unitOfWork.Complete())
            {
                
                return CreatedAtRoute("GetUser",new{username=username},_mapper.Map<PhotoDto>(photo));
            }
            else{
                return BadRequest("Problem adding photo");
            }
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user=await _unitOfWork.UserRespository.GetUserByUserNameAsync(User.GetUserName());
            var photo=user.Photos.FirstOrDefault(x=>x.ID==photoId);
            if(photo.IsMain) return BadRequest("Selected photo is already main");
            //set current main to false
            var currentMainPhoto=user.Photos.FirstOrDefault(x=>x.IsMain);
            if(currentMainPhoto!=null) {
                currentMainPhoto.IsMain=false;
            }
            //set new main photo
            photo.IsMain=true;
            if(await _unitOfWork.Complete()) return NoContent();
            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user=await _unitOfWork.UserRespository.GetUserByUserNameAsync(User.GetUserName());
            var photo=user.Photos.FirstOrDefault(x=>x.ID==photoId); //get photo of login user with given photoid
            if(photo==null) return NotFound();
            if(photo.IsMain) return BadRequest("Can't delete main photo");
            if(photo.PublicId!=null)
            {
                var result=await _photoService.DeletePhotoAsync(photo.PublicId); //remove from cloudinary
                if(result.Error!=null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo); //remove from db
            if(await _unitOfWork.Complete()) return Ok();
            return BadRequest("Fail to delete the photo");
        }
    }
}