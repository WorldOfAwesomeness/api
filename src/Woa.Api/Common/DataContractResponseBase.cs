using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace Woa.Api.Common
{
    [DataContract]
    public record DataContractResponseBase : IResult
    {
        public DataContractResponseBase(ResultStatuses status)
        {
            Status = status;
        }

        public DataContractResponseBase(Exception ex)
        {
            Status = ResultStatuses.Error;
            Exception = ex;
        }

        [DataMember]
        public ResultStatuses Status { get; init; }

        [DataMember]
        public Exception? Exception { get; init; }

        [DataMember]
        public string? SerializedData { get; private set; }

        [DataMember]
        public string? TypeName { get; private set; }

        public DataContractResponseBase SetData<TData>(TData? data)
        {
            TypeName = typeof(TData).FullName;
            SerializedData = data.ToJson();
            _data = data;

            return this;
        }

        private object? _data = null;

        public object? Deserialize(JsonSerializerSettings? settings = default)
        {
            if (TypeName is null or "" || SerializedData is null or "")
            {
                return null;
            }

            var data = SerializedData.FromJson(TypeName, settings);

            return data;

        }
        public TDeserialized? Deserialize<TDeserialized>(JsonSerializerSettings? settings = default)
            where TDeserialized : new()
        {
            var data = SerializedData.FromJson<TDeserialized?>(settings);

            return data;
        }

        protected TData? GetData<TData>() => (TData?)_data;

        /// <summary>Write an HTTP response reflecting the result.</summary>
        /// <param name="httpContext">The <see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> for the current request.</param>
        /// <returns>A task that represents the asynchronous execute operation.</returns>
        public Task ExecuteAsync(HttpContext context)
        {
            context.Response.Headers.Add(
                "Content-Type",
                new StringValues(
                    new[]
                    {
                        "application/json", 
                        "charset=utf-8",
                    }
                )
            );

            var ok = Results.Ok(this);

            return context.Response.WriteAsJsonAsync(ok, ok.GetType());
        }
    }
}
