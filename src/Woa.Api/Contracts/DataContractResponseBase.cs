using System.Runtime.Serialization;
using Newtonsoft.Json;
using Woa.Api.Common;

namespace Woa.Api.Startup
{
    [DataContract]
    public record DataContractResponseBase
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

            return this;
        }

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
    }
}
