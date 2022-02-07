using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class LikeDto
    {
        public int Id{get;set;}
        public string userName{get;set;}
        public int age{get;set;}
        public string knowAs{get;set;}
        public string photoUrl{get;set;}
        public string city{get;set;}
    }
}