namespace MeiliSearch
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MeiliSearch.Dto;
    using RestSharp;

    public class RawClient
    {
        readonly RestClient rest;
        readonly Config config;

        public RawClient(Config config)
        {
            this.config = config;
            this.rest = new RestClient(config.Host);
        }

        public Task<List<IndexResponse>> ListIndexesAsync() =>
            rest.GetAsync<List<IndexResponse>>(new RestRequest("/indexes", DataFormat.Json));

        public async Task<IndexResponse> CreateIndexAsync(CreateIndexRequest data)
        {
            var response = await rest.PostAsync<List<IndexResponse>>(
                new RestRequest("/indexes", DataFormat.Json).AddJsonBody(data))
                .ConfigureAwait(false);
            return response.Single();
        }

        public Task<IndexResponse> UpdateIndexAsync(string uid, UpdateIndexRequest updateRequest) =>
            rest.PutAsync<IndexResponse>(
                new RestRequest($"/indexes/{uid}").AddJsonBody(updateRequest));

        public Task<IndexResponse> GetIndexAsync(string uid) =>
            rest.GetAsync<IndexResponse>(new RestRequest($"/indexes/{uid}"));

        public Task DeleteIndexAsync(string uid) =>
            rest.DeleteAsync<string>(new RestRequest($"/indexes/{uid}"));
    }
}
