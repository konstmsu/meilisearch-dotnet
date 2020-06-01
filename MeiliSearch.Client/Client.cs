using System;
using System.Collections.Generic;
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
        readonly RestClient rest;
        readonly Config config;

        public RawClient2(Config config)
        {
            this.config = config;
            this.rest = new RestClient(config.host);
        }

        public Task<List<IndexResponse>> ListIndexes() =>
            rest.GetAsync<List<IndexResponse>>(new RestRequest("/indexes", DataFormat.Json));

        public Task<List<IndexResponse>> CreateIndex(IndexRequest data) =>
            rest.PostAsync<List<IndexResponse>>(new RestRequest("/indexes", DataFormat.Json).AddJsonBody(data));

        public Task<IndexResponse> GetIndex(string uid) =>
            rest.GetAsync<IndexResponse>(new RestRequest($"/indexes/{uid}"));

        public Task DeleteIndex(string uid) =>
            rest.DeleteAsync<string>(new RestRequest($"/indexes/{uid}"));
    }

    public class Client2
    {
        readonly RawClient2 client;

        public Client2(RawClient2 client) => this.client = client;

        public Task DeleteIndex(string uid) => client.DeleteIndex(uid);

        public Task<List<IndexResponse>> ListIndexes() => client.ListIndexes();

        public Task<List<IndexResponse>> CreateIndex(IndexRequest data) => client.CreateIndex(data);

        public Task<IndexResponse> GetIndex(string uid) => client.GetIndex(uid);
    }
}
