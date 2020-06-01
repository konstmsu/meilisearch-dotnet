namespace MeiliSearch
{
    public class Config
    {
        public Config(string host, string? apiKey = null)
        {
            Host = host;
            ApiKey = apiKey;
        }

        public string Host { get; }
        public string? ApiKey { get; }
    }
}
