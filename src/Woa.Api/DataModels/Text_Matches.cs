namespace Woa.Api.DataModels;

public class TextMatches
{
    public string? ObjectUrl
    {
        get; set;
    }
    public string? ObjectType
    {
        get; set;
    }
    public string? Property
    {
        get; set;
    }
    public string? Fragment
    {
        get; set;
    }
    public Match[]? Matches
    {
        get; set;
    }
}