namespace Woa.Api.Services;

public class IndexPageService
    : ServiceContractBase<DataContractBase, MarkdownResult>
{
    public override string RelativePath => "/";

    public IndexPageService(IServiceProvider serviceProvider) 
        : base(serviceProvider)
    {
    }

    internal override Task<MarkdownResult?> Operation(
        HttpContext context, 
        DataContractBase? request) 
        => Task.FromResult(new MarkdownResult(File.ReadAllText("./Markdown/index.md")));
}