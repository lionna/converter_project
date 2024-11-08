using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApi
{
    public class ErrorDto
    {
        [JsonPropertyName("errorCode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("result")]
        public object Result { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}