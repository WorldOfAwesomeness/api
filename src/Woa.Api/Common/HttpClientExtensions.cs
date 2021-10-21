using System.Net.Http;

namespace Woa.Api.Common;

public static class HttpClientExtensions
{
    public static HttpClient AddHeaders(this HttpClient httpClient)
    {
        httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.text-match+json");
        httpClient.DefaultRequestHeaders.Add("user-agent", "WorldOfAwesomeness");
        return httpClient;
    }
}
