using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<List<IndexResponse>> ListIndexes()
        {
            var request = new RestRequest("/indexes", DataFormat.Json);
            var response = await rest.GetAsync<List<IndexResponse>>(request);
            return response;
        }

        public async Task<List<IndexResponse>> CreateIndex(IndexRequest data)
        {
            var request = new RestRequest("/indexes", DataFormat.Json).AddJsonBody(data);
            var response = await rest.PostAsync<List<IndexResponse>>(request);
            return response;
        }

        public async Task<IndexResponse> GetIndex(string uid)
        {
            var request = new RestRequest($"/indexes/{uid}");
            var response = await rest.GetAsync<IndexResponse>(request);
            return response;
        }

        public async Task DeleteIndex(string uid)
        {
            var request = new RestRequest($"/indexes/{uid}");
            await rest.DeleteAsync<string>(request);
        }
    }

    public class Client2
    {
        readonly RawClient2 client;

        public Client2(RawClient2 client)
        {
            this.client = client;
        }

        public Task DeleteIndex(string uid)
        {
            return client.DeleteIndex(uid);
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

        public Task<IndexResponse> GetIndex(string uid) => client.GetIndex(uid);
    }
}
