namespace MeiliSearch
{
    using System;
    using System.Threading.Tasks;

    static class TestUtils
    {
        public static readonly TimeSpan TolerableTimeDifference = TimeSpan.FromSeconds(2);

        public static async Task DeleteAllIndexesAsync(Client client)
        {
            foreach (var index in await client.ListIndexesAsync().ConfigureAwait(false))
                await client.DeleteIndexAsync(index.uid).ConfigureAwait(false);
        }
    }
}
