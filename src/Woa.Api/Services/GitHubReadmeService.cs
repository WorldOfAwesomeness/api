namespace Woa.Api.Services;

public class GitHubReadmeService 
    : ServiceContractBase<DisplayReadmeRequest, GitHubReadmeResult>
{
    private const string HTTPS_API_GITHUB_COM_README 
        = "https://api.github.com/repos/{owner}/{repo}/readme";

    public override string RelativePath => "/api/Readme";

    public GitHubReadmeService(
        IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }

    internal override async Task<GitHubReadmeResult?> Operation(
        HttpContext context, 
        DisplayReadmeRequest? request)
    {
        var httpClient = ServiceProvider.GetService<HttpClient>();

        httpClient!.AddHeaders();

        var q = $"{request?.Id}";

        var item = request.Item;

        var uri = HTTPS_API_GITHUB_COM_README
            .Replace("{owner}", item.Owner.Login)
            .Replace("{repo}", item.Name);

        Console.WriteLine(uri);

        var result = await httpClient!.GetStringAsync(uri).ConfigureAwait(false);

        var data = result.FromJson<ReadmeResult>();

        return new GitHubReadmeResult(ResultStatuses.Success)
        {
            Data = data,
        };
    }
}