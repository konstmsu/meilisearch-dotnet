using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Xunit;
using static MeiliSearch.TestUtils;

namespace MeiliSearch
{
    public class ClientTests
    {
        IndexRequest uidNoPrimaryKey = new IndexRequest { uid = "movies_test" };
        IndexRequest uidAndPrimaryKey = new IndexRequest
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

            var shown = await client.GetIndex(request.uid);

            shown.Should().BeEquivalentTo(new IndexResponse { uid = request.uid, name = request.uid, createdAt = DateTimeOffset.Now, updatedAt = DateTimeOffset.Now }, Tolerant);

            shown.uid.Should().Be(request.uid);
            shown.name.Should().Be(request.uid);
            shown.primaryKey.Should().Be(null);
            shown.createdAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));
            shown.updatedAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));
        });

        static EquivalencyAssertionOptions<TEx> Tolerant<TEx>(EquivalencyAssertionOptions<TEx> options) =>
            options.Using<DateTimeOffset>(context => context.Subject.Should().BeCloseTo(context.Expectation, TimeSpan.FromSeconds(1))).WhenTypeIs<DateTimeOffset>();
    }
}
