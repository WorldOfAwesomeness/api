using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Woa.Api.Startup
{
    [DataContract]
    public record DataContractBase()
    {

    }

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
        public string? SerializedData { get; init; }

        [DataMember]
        public string? TypeName { get; init; }

        public object? Deserialize(JsonSerializerSettings? settings = default)
        {
            Type? type = Type.GetType(TypeName);
            if (type is not null && SerializedData is not (null or ""))
            {
                var data = JsonConvert.DeserializeObject(SerializedData, type, settings);

                return data;
            }

            return null;
        }
        public TDeserialized? Deserialize<TDeserialized>(JsonSerializerSettings? settings = default)
        {
            var data = JsonConvert.DeserializeObject<TDeserialized>(SerializedData, settings);

            return data;
        }
    }

    public enum ResultStatuses
    {
        Unknown, Success, Failure, Error
    }
}
