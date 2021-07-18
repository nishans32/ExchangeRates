using System.Text.Json;

namespace ExchangeRates.Processor.Lib
{
    public interface IJsonSerializer
    {
        T Deserialize<T>(string source);
    }

    /// <summary>
    /// This is a wrapper for Json Serializer
    /// </summary>
    public class JsonSerializerService : IJsonSerializer
    {
        public T Deserialize<T>(string source)
        {
            return JsonSerializer.Deserialize<T>(source);
        }
    }
}