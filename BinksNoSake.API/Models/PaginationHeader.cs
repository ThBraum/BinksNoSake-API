using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinksNoSake.API.Models;
public class PaginationHeader
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public PaginationHeader(int currentPage, int totalItems, int PageSize, int totalPages)
    {
        this.CurrentPage = currentPage;
        this.TotalItems = totalItems;
        this.PageSize = PageSize;
        this.TotalPages = totalPages;
    }
}