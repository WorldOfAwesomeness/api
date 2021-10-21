namespace Woa.Api.Contracts;

public interface IServiceContract
{
    string RelativePath
    {
        get;
    }

    Methods Method => Methods.Get;

    IEndpointConventionBuilder Register(WebApplication app, int order);
}
