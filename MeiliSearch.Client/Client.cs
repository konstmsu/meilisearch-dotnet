using MeiliSearch.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeiliSearch
{
    public class Client
    {
        readonly RawClient client;

        public Client(RawClient client) => this.client = client;

        public Task DeleteIndexAsync(string uid) => client.DeleteIndexAsync(uid);

        public Task<List<IndexResponse>> ListIndexesAsync() => client.ListIndexesAsync();

        public Task<IndexResponse> CreateIndexAsync(CreateIndexRequest data) => client.CreateIndexAsync(data);

        public Task<IndexResponse> GetIndexAsync(string uid) => client.GetIndexAsync(uid);

        public Task<IndexResponse> UpdateIndexAsync(string uid, UpdateIndexRequest updateRequest) => client.UpdateIndexAsync(uid, updateRequest);
    }
}
