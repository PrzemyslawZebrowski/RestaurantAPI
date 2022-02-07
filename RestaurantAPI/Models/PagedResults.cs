using System;
using System.Collections.Generic;

namespace RestaurantAPI.Models;

public class PagedResults<T>
{
    public PagedResults(List<T> items, int pageSize, int totalItemCount, int pageNumbers)
    {
        Items = items;
        ItemFrom = pageSize * (pageNumbers - 1) + 1;
        TotalItemCount = totalItemCount;
        ItemTo = ItemFrom + pageSize - 1 > TotalItemCount ? TotalItemCount : ItemFrom + pageSize - 1;
        TotalPages = (int) Math.Ceiling((decimal) TotalItemCount / pageSize);
    }

    public List<T> Items { get; set; }
    public int TotalPages { get; set; }
    public int ItemFrom { get; set; }
    public int ItemTo { get; set; }
    public int TotalItemCount { get; set; }
}