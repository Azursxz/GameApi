using System;

/// <summary>
/// Summary description for Class1
/// </summary>
public static class PageMapper
{
    public static PagedResult<T> ToPagedResult<T>(
        IEnumerable<T> items,
        int totalItems,
        int pageNumber,
        int pageSize)
    {
        return new PagedResult<T>
        {
            TotalItems = totalItems,
            PageSize = pageSize,
            PageNumber = pageNumber,
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
            Items = items.ToList()
        };
    }
}
