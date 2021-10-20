namespace Woa.Api.Startup;

public class RepositorySearchResult
{
    public int total_count
    {
        get; set;
    }
    public int item_count
    {
        get; set;
    }
    public bool incomplete_results
    {
        get; set;
    }
    public Item[] items
    {
        get; set;
    }
}