namespace MeiliSearch
{
    public class Config
    {
        public readonly string host;
        public readonly string? apiKey;

        public Config(string host, string? apiKey = null)
        {
            this.host = host;
            this.apiKey = apiKey;
        }
    }
}
