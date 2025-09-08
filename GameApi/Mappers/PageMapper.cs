using System;
using GameApi.DTO;

/// <summary>
/// Summary description for Class1
/// </summary>
public static class PageMapper
{
    public static PageResult<T> ToPagedResult<T>(
        IEnumerable<T> items,
        int totalItems,
        int pageNumber,
        int pageSize)
    {
        return new PageResult<T>
        {
            TotalItems = totalItems,
            PageSize = pageSize,
            PageNumber = pageNumber,
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
            Items = items.ToList()
        };
    }
}
