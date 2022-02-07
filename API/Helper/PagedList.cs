using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Helper
{
    public class PagedList<T> :List<T>
    {
        public PagedList(IEnumerable<T> items,int count,int pagenumber, int pagesize)
        {
            currentPage = pagenumber;
            totalPages = (int) Math.Ceiling(count/(double) pagesize );
            pageSize = pagesize;
            totalCount = count;
            AddRange(items);
        }

        public int currentPage{get;set;}
        public int pageSize{get;set;}
        public int totalPages{get;set;}

        public int totalCount{get;set;}

        public static async Task<PagedList<T>> CreateAsync (IQueryable<T> source,int pagenumber,int pagesize)
        {
            var count = await source.CountAsync();
            var items=await source.Skip((pagenumber-1)*pagesize).Take(pagesize).ToListAsync();
            //for page 1 skip none take first pagesize data
            //for page 2 skip 1*pagesize data take after data
            return new PagedList<T>(items,count,pagenumber,pagesize);
        }
    }
}