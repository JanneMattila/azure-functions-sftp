using System.Text.Json.Serialization;

namespace FuncSFTP.Interfaces
{
    public class GetFileRequest
    {
        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;
    }
}
