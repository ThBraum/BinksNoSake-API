using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BinksNoSake.API.Models;

namespace BinksNoSake.API.Extensions;
public static class Pagination
{
    public static void AddPagination(this HttpResponse response, int currentPage, int pageSize, int totalItems, int totalPages)
    {
        var pagination = new PaginationHeader(currentPage, totalItems, pageSize, totalPages);
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        response.Headers.Add("Pagination", JsonSerializer.Serialize(pagination, options));
        response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
    }
}