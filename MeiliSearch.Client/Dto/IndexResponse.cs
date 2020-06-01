using System;

namespace MeiliSearch
{
    public class IndexResponse
    {
        public string uid { get; set; }
        public string? name { get; set; }
        public string? primaryKey { get; set; }
        public DateTimeOffset createdAt { get; set; }
        public DateTimeOffset updatedAt { get; set; }
    }
}
