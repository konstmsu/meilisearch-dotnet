using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace MeiliSearch
{
    public class RawClient
    {
        readonly RestClient rest;
        readonly Config config;

        public RawClient(Config config)
        {
            this.config = config;
            this.rest = new RestClient(config.host);
        }

        public Task<List<IndexResponse>> ListIndexes() =>
            rest.GetAsync<List<IndexResponse>>(new RestRequest("/indexes", DataFormat.Json));

        public async Task<IndexResponse> CreateIndex(IndexRequest data)
        {
            var response = await rest.PostAsync<List<IndexResponse>>(new RestRequest("/indexes", DataFormat.Json).AddJsonBody(data));
            return response.Single();
        }

        public Task<IndexResponse> GetIndex(string uid) =>
            rest.GetAsync<IndexResponse>(new RestRequest($"/indexes/{uid}"));

        public Task DeleteIndex(string uid) =>
            rest.DeleteAsync<string>(new RestRequest($"/indexes/{uid}"));
    }
}
