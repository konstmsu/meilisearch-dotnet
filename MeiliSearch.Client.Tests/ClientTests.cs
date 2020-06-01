using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

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

        static Client2 CreateClient()
        {
            return new Client2(new RawClient2(new Config("http://ko-ub.southeastasia.cloudapp.azure.com:7700/")));
        }

        [Fact]
        public async Task ListIndexes_Empty()
        {
            await DeleteAllIndexes();

            var client = CreateClient();
            (await client.ListIndexes()).Should().BeEmpty();
        }

        async Task DeleteAllIndexes()
        {
            var client = CreateClient();
            foreach (var index in await client.ListIndexes())
            {
                await client.DeleteIndex(index.uid);
            }
        }

        [Fact]
        public async Task ListIndexes_NoPrimaryKey()
        {
            var client = CreateClient();
            var request = uidNoPrimaryKey;

            var createResponse = await client.CreateIndex(request);
            createResponse.Single().uid.Should().Be(request.uid);

            var shown = await client.GetIndex(request.uid);

            shown.Should().BeEquivalentTo(new { request.uid, name=request.uid, primaryKey=(string)null, createdAt=DateTimeOffset.Now }, options =>
            {
                options.Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1))).WhenTypeIs<DateTimeOffset>();
                return options;
            });

            shown.uid.Should().Be(request.uid);
            shown.name.Should().Be(request.uid);
            shown.primaryKey.Should().Be(null);
            shown.createdAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));
            shown.updatedAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));
        }
    }
}
