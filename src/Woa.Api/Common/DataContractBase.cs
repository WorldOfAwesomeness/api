
namespace Woa.Api.Common;

[DataContract]
public record DataContractBase()
{
    [DataMember]
    public virtual string? BodyContents { get; set; }

    [JsonIgnore]
    public virtual string? SidebarContents { get; set; }
}
