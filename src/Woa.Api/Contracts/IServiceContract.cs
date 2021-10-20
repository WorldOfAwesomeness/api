using Microsoft.AspNetCore.Builder;

namespace Woa.Api.Startup;

public interface IServiceContract
{
    string RelativePath
    {
        get;
    }
    IEndpointConventionBuilder Register(WebApplication app,
        Methods method,
        string path,
        int order);
}
