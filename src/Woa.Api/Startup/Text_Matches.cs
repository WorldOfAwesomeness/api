namespace Woa.Api.Startup;

public class Text_Matches
{
    public string object_url
    {
        get; set;
    }
    public string object_type
    {
        get; set;
    }
    public string property
    {
        get; set;
    }
    public string fragment
    {
        get; set;
    }
    public Match[] matches
    {
        get; set;
    }
}