namespace MeiliSearch.Dto
{
    public class CreateIndexRequest
    {
        public string uid { get; set; }
        public string? primaryKey { get; set; }
    }

    public class UpdateIndexRequest
    {
        public string? primaryKey { get; set; }
    }
}
