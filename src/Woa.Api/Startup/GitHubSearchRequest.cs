using System.Runtime.Serialization;

namespace Woa.Api.Startup
{
    public record GitHubSearchRequest : DataContractBase
    {

        [DataMember]
        public string? Query { get; set; }
    }
}