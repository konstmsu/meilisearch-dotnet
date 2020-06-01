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

        public Task DeleteIndex(string uid) => client.DeleteIndexAsync(uid);

        public Task<List<IndexResponse>> ListIndexes() => client.ListIndexesAsync();

        public Task<IndexResponse> CreateIndex(CreateIndexRequest data) => client.CreateIndexAsync(data);

        public Task<IndexResponse> GetIndex(string uid) => client.GetIndexAsync(uid);

        public Task<IndexResponse> UpdateIndex(string uid, UpdateIndexRequest updateRequest) => client.UpdateIndexAsync(uid, updateRequest);
    }
}
