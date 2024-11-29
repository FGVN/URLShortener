﻿namespace URLShortener.Server.Controllers;

public class PagedResult<T>
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public IEnumerable<T> Data { get; set; }
}
