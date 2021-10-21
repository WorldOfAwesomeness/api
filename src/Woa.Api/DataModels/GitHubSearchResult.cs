using Woa.Api.Common;

namespace Woa.Api.DataModels;

public record GitHubSearchResult : DataContractResponseBase
{
    public GitHubSearchResult() : base(ResultStatuses.Success)
    { }
    public GitHubSearchResult(ResultStatuses status) : base(status)
    { }
    public GitHubSearchResult(Exception ex) : base(ex)
    { }

    public object? Data
    {
        get => GetData<RepositorySearchResult>();
        set => SetData(value);
    }
}