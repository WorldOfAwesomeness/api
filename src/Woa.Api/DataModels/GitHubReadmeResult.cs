namespace Woa.Api.DataModels;

public record GitHubReadmeResult : DataContractResponseBase
{
    public GitHubReadmeResult() : base(ResultStatuses.Success)
    { }
    public GitHubReadmeResult(ResultStatuses status) : base(status)
    { }
    public GitHubReadmeResult(Exception ex) : base(ex)
    { }

    public object? Data
    {
        get => GetData<ReadmeResult>();
        set => SetData(value);
    }

    public ReadmeResult? ReadmeData => Data as ReadmeResult;
}