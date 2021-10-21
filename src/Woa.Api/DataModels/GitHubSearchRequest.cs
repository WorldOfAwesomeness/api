namespace Woa.Api.DataModels;

public record GitHubSearchRequest : DataContractBase
{

    [DataMember]
    public string? Query { get; set; }

    [JsonIgnore]
    public override string? BodyContents
    {
        get => Query;
        set => Query = value;
    }
}