namespace Woa.Api.DataModels;

public record DisplayReadmeRequest : DataContractBase
{
    [DataMember]
    public int? Id { get; set; }

    [JsonIgnore]
    public override string? BodyContents
    {
        get => $"{Id}";
        set => Id = int.TryParse(value, out var data) ? data : null;
    }

    [JsonIgnore]
    internal Item? Item { get; set; }
}