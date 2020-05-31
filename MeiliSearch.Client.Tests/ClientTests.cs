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
            return new Client2(new RawClient2(new Config("http://localhost:7700")));
        }

        [Fact]
        public async Task ListIndexes_Empty()
        {
            var client = CreateClient();
            (await client.ListIndexes()).Should().BeEmpty();
        }

        [Fact(Skip =true)]
        public async Task ListIndexes_NoPrimaryKey()
        {
            var client = CreateClient();
            var request = uidNoPrimaryKey;

            var createResponse = await client.CreateIndex(request);
            createResponse.Single().uid.Should().Be(request.uid);

            var shown = await client.GetIndex(request.uid).Show();

            shown.uid.Should().Be("uid");
            shown.primaryKey.Should().Be(null);
            shown.createdAt.Should().BeCloseTo(DateTimeOffset.Now);
            shown.updatedAt.Should().BeCloseTo(DateTimeOffset.Now);
        }
    }
}
