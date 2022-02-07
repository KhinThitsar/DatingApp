using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helper
{
    public class PaginationParams
    {
         private int maxPageSize=50;
        public int pageNumber{get;set;}=1;
        private int _pageSize=5;
        public int pageSize{
            get=>_pageSize;
            set=>_pageSize=(value>maxPageSize) ? maxPageSize : value; //if user choose value is greater than maxpagesize set page size to max else set choose value
        }
    }
}