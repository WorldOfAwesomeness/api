using System.ComponentModel;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Woa.Api.Startup
{
    public interface IServiceContract
    {
        IEndpointConventionBuilder Register(WebApplication app, Methods method, string path);
    }

    [ServiceContract]
    public abstract class ServiceContractBase<TRequest, TResult> 
        : IServiceContract 
            where TRequest : DataContractBase
            where TResult : DataContractResponseBase, new()
    {
        [OperationContract]
        protected abstract Task<TResult?> Operation(TRequest request);

        public IEndpointConventionBuilder Register(WebApplication app, Methods method, string path)
        {
            return method switch
            {
                Methods.Get => app.MapGet(path, Proxy),
                _ => app.MapPost(path, Proxy),
            };
        }

        private async Task<TResult?> Proxy(HttpContext context)
        {
            var json = await context.Request.BodyReader.ReadAsync();
            var request = JsonConvert.DeserializeObject<TRequest>(json.Buffer.ToString());

            if (request is not null)
            {
                return await Operation(request);
            }

            return default;
        }
    }

    public enum Methods {
        Get, Post
    }
}
