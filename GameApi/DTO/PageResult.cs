using System;

/// <summary>
/// Summary description for Class1
/// </summary>
public class PageResult<T>
{
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; } = new();
 
}
