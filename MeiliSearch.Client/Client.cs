using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace MeiliSearch
{
    public class IndexRequest
    {
        public string uid { get; set; }
        public string? primaryKey { get; set; }
    }

    public class IndexResponse
    {
        public string uid { get; set; }
        public string? name { get; set; }
        public string? primaryKey { get; set; }
        public DateTimeOffset createdAt { get; set; }
        public DateTimeOffset updatedAt { get; set; }
    }

    public class Index2
    {
        public Task<IndexResponse> Show()
        {
            throw new NotImplementedException();
        }
    }

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

    public class RawClient2
    {
        readonly RestClient rest = new RestClient("http://localhost:7700");
        readonly Config config;

        public RawClient2(Config config)
        {
            this.config = config;
        }

        public async Task<List<IndexResponse>> ListIndexes()
        {
            return new List<IndexResponse>();
        }

        public async Task<List<IndexResponse>> CreateIndex(IndexRequest data)
        {
            var request = new RestRequest("/indexes", DataFormat.Json);
            var response = await rest.GetAsync<List<IndexResponse>>(request);
            return response;
        }

        public Index2 GetIndex(string uid)
        {
            return new Index2();
        }
    }

    public class Client2
    {
        readonly RawClient2 client;

        public Client2(RawClient2 client)
        {
            this.client = client;
        }

        public Task<List<IndexResponse>> ListIndexes()
        {
            return client.ListIndexes();
        }

        public async Task<List<IndexResponse>> CreateIndex(IndexRequest data)
        {
            var result = await client.CreateIndex(data);
            return result;
        }

        public Index2 GetIndex(string uid)
        {
            return client.GetIndex(uid);
        }
    }
}
