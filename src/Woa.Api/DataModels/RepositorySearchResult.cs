namespace Woa.Api.DataModels;

public class RepositorySearchResult
{
    public int? TotalCount
    {
        get; set;
    }
    public int? ItemCount
    {
        get; set;
    }
    public bool? IncompleteResults
    {
        get; set;
    }
    public Item[]? Items
    {
        get; set;
    }
}