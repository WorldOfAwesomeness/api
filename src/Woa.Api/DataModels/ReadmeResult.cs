namespace Woa.Api.DataModels;

public class ReadmeResult
{
    public string? Name { get; set; }
    public string? Path { get; set; }
    public string? Sha { get; set; }
    public int? Size { get; set; }
    public string? Url { get; set; }
    public string? HtmlUrl { get; set; }
    public string? GitUrl { get; set; }
    public string? DownloadUrl { get; set; }
    public string? Type { get; set; }
    public string? Content { get; set; }
    public string? Encoding { get; set; }
    public Links? Links { get; set; }
}