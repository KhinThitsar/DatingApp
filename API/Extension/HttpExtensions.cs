using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.Helper;

namespace API.Extension
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader (this HttpResponse response,int currentPage,int itemPerPage,int totalPages,int totalItems)
        {
            var paginationHeader=new PaginationHeaders(currentPage,itemPerPage,totalPages,totalItems);
            var options=new JsonSerializerOptions{
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase
            };
            response.Headers.Add("Pagination",JsonSerializer.Serialize(paginationHeader,options));
            response.Headers.Add("Access-Control-Expose-Headers","Pagination");
        }
        
    }
}