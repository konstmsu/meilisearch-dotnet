namespace MeiliSearch
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using FluentAssertions.Equivalency;
    using MeiliSearch.Dto;
    using Xunit;
    using static MeiliSearch.TestUtils;

    public class ClientTests
    {
        readonly CreateIndexRequest uidNoPrimaryKey = new CreateIndexRequest { uid = "movies_test" };
        readonly CreateIndexRequest uidAndPrimaryKey = new CreateIndexRequest
        {
            uid = "movies_test2",
            primaryKey = "id",
        };

        static Client CreateClient()
        {
            return new Client(new RawClient(new Config("http://ko-ub.southeastasia.cloudapp.azure.com:7700/")));
        }

        static async Task RunInClean(Func<Client, Task> action)
        {
            var client = CreateClient();
            await DeleteAllIndexesAsync(client);
            await action(client);
        }

        [Fact]
        public Task ListIndexes_Empty() => RunInClean(async client =>
        {
            (await client.ListIndexesAsync()).Should().BeEmpty();
        });

        [Fact]
        public Task ListIndexes_NoPrimaryKey() => RunInClean(async client =>
        {
            var request = this.uidNoPrimaryKey;

            var createResponse = await client.CreateIndexAsync(request);
            createResponse.uid.Should().Be(request.uid);

            var index = await client.GetIndexAsync(request.uid);

            index.uid.Should().Be(request.uid);
            index.name.Should().Be(request.uid);
            index.primaryKey.Should().Be(null);
            index.createdAt.Should().BeCloseTo(DateTimeOffset.Now, TolerableTimeDifference);
            index.updatedAt.Should().BeCloseTo(DateTimeOffset.Now, TolerableTimeDifference);
        });

        [Fact]
        public Task CreateIndexWithPrimaryKey() => RunInClean(async client =>
        {
            var request = uidAndPrimaryKey;
            var response = await client.CreateIndexAsync(request);
            var index = await client.GetIndexAsync(request.uid);
            index.uid.Should().Be(request.uid);
            index.name.Should().Be(request.uid);
            index.primaryKey.Should().NotBeNullOrWhiteSpace();
            index.createdAt.Should().BeCloseTo(DateTimeOffset.Now, TolerableTimeDifference);
            index.updatedAt.Should().BeCloseTo(DateTimeOffset.Now, TolerableTimeDifference);
        });

        [Fact]
        public Task ListIndexesNotEmpty() => RunInClean(async client =>
        {
            var requests = new[] { uidAndPrimaryKey, uidNoPrimaryKey };
            var tasks = requests.Select(r => client.CreateIndexAsync(r)).ToList();
            var expected = await Task.WhenAll(tasks);
            var actual = await client.ListIndexesAsync();
            actual.Select(a => a.uid).Should().BeEquivalentTo(tasks.Select(t => t.Result.uid));
        });

        [Fact]
        public Task UpdateSetPrimaryKey() => RunInClean(async client =>
        {
            var request = uidNoPrimaryKey;
            var index = await client.CreateIndexAsync(request);
            // TODO uid is in request. Maybe it shouldn't?
            var updated = await client.UpdateIndexAsync(request.uid, new UpdateIndexRequest { primaryKey = "newPrimaryKey" });
            var actual = await client.GetIndexAsync(request.uid);
            actual.uid.Should().Be(index.uid);
            actual.primaryKey.Should().Be("newPrimaryKey");
        });
    }
}
