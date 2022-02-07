using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extension;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helper
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result=await next(); 
            if(!result.HttpContext.User.Identity.IsAuthenticated) return;
            var userId=result.HttpContext.User.GetUserID();
            var repo=result.HttpContext.RequestServices.GetService<IUnitOfWork>();
            var user=await repo.UserRespository.GetUserByIDAsync(userId);
            user.LastActive=DateTime.UtcNow;
            await repo.Complete();
        }
    }
}