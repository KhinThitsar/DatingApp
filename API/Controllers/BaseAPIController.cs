using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    public class BaseAPIController :ControllerBase
    {
        
    }
}