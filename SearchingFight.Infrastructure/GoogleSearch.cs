using Newtonsoft.Json;
using SearchingFight.Core.Entities;
using SearchingFight.Core.Interfaces;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SearchingFight.Infrastructure
{
    public class GoogleSearch : ISearchEngine
    {
        private readonly IHttpClientFactory _clientFactory;

        public GoogleSearch(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public string Name => "Google";

        public async Task<EngineReponse> FindEntries(string search, CancellationTokenSource cancellationToken)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync($"https://app.zenserp.com/api/v2/search?q={search}&hl=en&gl=US&location=United States&search_engine=google.com&apikey=9607aa90-0fc5-11ea-928e-b35d449139d3", cancellationToken.Token);
                response.EnsureSuccessStatusCode();
                var bodyResponse = await response.Content.ReadAsStringAsync();
                var googleResponse = JsonConvert.DeserializeObject<GoogleResponse>(bodyResponse);
                return new EngineReponse() { Entries = googleResponse.number_of_results, Query = search, SearchEngine = this.Name };
            }
            catch (Exception)
            {
                cancellationToken.Cancel();
                throw;
            }
        }
    }
}
