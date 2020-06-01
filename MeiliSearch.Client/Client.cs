using MeiliSearch.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeiliSearch
{
    public class Client
    {
        readonly RawClient client;

        public Client(RawClient client) => this.client = client;

        public Task DeleteIndex(string uid) => client.DeleteIndex(uid);

        public Task<List<IndexResponse>> ListIndexes() => client.ListIndexes();

        public Task<IndexResponse> CreateIndex(IndexRequest data) => client.CreateIndex(data);

        public Task<IndexResponse> GetIndex(string uid) => client.GetIndex(uid);
    }
}
