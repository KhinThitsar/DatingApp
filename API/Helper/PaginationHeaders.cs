using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helper
{
    public class PaginationHeaders
    {
        public PaginationHeaders(int currentpage, int itemperpage, int totalpages, int totalitems)
        {
            currentPage = currentpage;
            itemPerPage = itemperpage;
            totalPages = totalpages;
            totalItems = totalitems;
        }

        public int currentPage{get;set;}
        public int itemPerPage{get;set;}
        public int totalPages{get;set;}
        public int totalItems{get;set;}
    }
}