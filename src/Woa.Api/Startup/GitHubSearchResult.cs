namespace Woa.Api.Startup
{
    public record GitHubSearchResult : DataContractResponseBase
    {
        public GitHubSearchResult() : base(ResultStatuses.Success)
        { }
        public GitHubSearchResult(ResultStatuses Status) : base(Status)
        { }
        public GitHubSearchResult(Exception ex) : base(ex)
        { }

        public object? Data { get; set; }
    }
}