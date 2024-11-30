namespace URLShortener.Core.Dtos;

public class PagedResult<T>
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public IEnumerable<T> Data { get; set; }

    public PagedResult(IEnumerable<T> data)
    {
        Data = data;
    }

    public PagedResult(int currentPage, int pageSize, int totalRecords, IEnumerable<T> data)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        Data = data;
    }
}
