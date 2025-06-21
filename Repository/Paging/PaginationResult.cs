namespace Repository.Paging;

public class PaginationResult
{
    public ICollection<object> Content { get; set; } = new List<object>();

    public int ItemAmount { get; set; }

    public int CurrentPage { get; set; }
    public int PageSize { get; set; }

    public int TotalPage => (int)Math.Ceiling((double)ItemAmount / PageSize);

}

public class PaginationResult<T> : PaginationResult
{
    new public ICollection<T> Content { get; set; } = new List<T>();
}
