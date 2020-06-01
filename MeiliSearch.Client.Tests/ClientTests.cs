using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using MeiliSearch.Dto;
using Xunit;
using static MeiliSearch.TestUtils;

namespace MeiliSearch
{
    public class ClientTests
    {
        CreateIndexRequest uidNoPrimaryKey = new CreateIndexRequest { uid = "movies_test" };
        CreateIndexRequest uidAndPrimaryKey = new CreateIndexRequest
        {
            uid = "movies_test2",
            primaryKey = "id"
        };

        static Client CreateClient()
        {
            return new Client(new RawClient(new Config("http://ko-ub.southeastasia.cloudapp.azure.com:7700/")));
        }

        static async Task RunInClean(Func<Client, Task> action)
        {
            var client = CreateClient();
            await DeleteAllIndexes(client);
            await action(client);
        }

        [Fact]
        public Task ListIndexes_Empty() => RunInClean(async client =>
        {
            (await client.ListIndexes()).Should().BeEmpty();
        });

        [Fact]
        public Task ListIndexes_NoPrimaryKey() => RunInClean(async client =>
        {
            var request = uidNoPrimaryKey;

            var createResponse = await client.CreateIndex(request);
            createResponse.uid.Should().Be(request.uid);

            var index = await client.GetIndex(request.uid);

            index.uid.Should().Be(request.uid);
            index.name.Should().Be(request.uid);
            index.primaryKey.Should().Be(null);
            index.createdAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));
            index.updatedAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));
        });

        [Fact]
        public Task CreateIndexWithPrimaryKey() => RunInClean(async client =>
        {
            var request = uidAndPrimaryKey;
            var response = await client.CreateIndex(request);
            var index = await client.GetIndex(request.uid);
            index.uid.Should().Be(request.uid);
            index.name.Should().Be(request.uid);
            index.primaryKey.Should().NotBeNullOrWhiteSpace();
            index.createdAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));
            index.updatedAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));
        });

        [Fact]
        public Task ListIndexesNotEmpty() => RunInClean(async client =>
        {
            var requests = new[] { uidAndPrimaryKey, uidNoPrimaryKey };
            var tasks = requests.Select(r => client.CreateIndex(r)).ToList();
            var expected = await Task.WhenAll(tasks);
            var actual = await client.ListIndexes();
            actual.Select(a => a.uid).Should().BeEquivalentTo(tasks.Select(t => t.Result.uid));
        });

        [Fact]
        public Task UpdateSetPrimaryKey() => RunInClean(async client =>
        {
            var request = uidNoPrimaryKey;
            var index = await client.CreateIndex(request);
            // TODO uid is in request. Maybe it shouldn't?
            var updated = await client.UpdateIndex(request.uid, new UpdateIndexRequest { primaryKey = "newPrimaryKey" });
            var actual = await client.GetIndex(request.uid);
            actual.uid.Should().Be(index.uid);
            actual.primaryKey.Should().Be("newPrimaryKey");
        });
    }
}
