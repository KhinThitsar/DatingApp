using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helper
{
    public class UserParams :PaginationParams
    {
       
        //for filtering
        public string currentUserName{get;set;}
        public string gender{get;set;}
        public int minAge{get;set;}=18;
        public int maxAge{get;set;}=150;
        public string orderBy{get;set;}="LastActive";

    }
}