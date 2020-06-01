using System;
using System.Threading.Tasks;

namespace MeiliSearch
{
    static class TestUtils
    {
        public static async Task DeleteAllIndexesAsync(Client client)
        {
            foreach (var index in await client.ListIndexes())
                await client.DeleteIndex(index.uid);
        }

        public static readonly TimeSpan TolerableTimeDifference = TimeSpan.FromSeconds(2);
    }
}
