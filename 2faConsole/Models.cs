using System.Text.Json.Serialization;

namespace _2faConsole
{
    public sealed class OtpFileDm
    {
        [JsonPropertyName("Otps")]
        public List<OtpDm> Otps { set; get; } = new List<OtpDm>();
    }
    public sealed class OtpDm
    {
        [JsonPropertyName("Id")]
        public string Id { set; get; } = string.Empty;

        [JsonPropertyName("Name")]
        public string Name { set; get; } = string.Empty;

        [JsonPropertyName("Secret")]
        public string Secret { set; get; } = string.Empty;

        [JsonPropertyName("Digits")]
        public int? Digits { set; get; } = 0;

        [JsonPropertyName("Type")]
        public OtpType Type { set; get; } = OtpType.Unknown;

        [JsonPropertyName("Counter")]
        public long? Counter { set; get; } = 0;

        [JsonPropertyName("Incremental")]
        public bool Incremental { set; get; } = false;
    }
    public enum OtpType
    {
        Unknown = 0,
        Totp = 1,
        Hotp = 2
    }
}
