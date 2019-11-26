using Microsoft.Azure.CognitiveServices.Search.WebSearch;
using SearchingFight.Core.Entities;
using SearchingFight.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SearchingFight.Infrastructure
{
    public class BingSearch : ISearchEngine
    {
        public string Name => "Bing";

        public async Task<EngineReponse> FindEntries(string search, CancellationTokenSource cancellationToken)
        {
            try
            {
                var client = new WebSearchClient(new ApiKeyServiceClientCredentials("90c0cb4ce7e34693a15c266bef5b43e4"));
                var webData = await client.Web.SearchAsync(query: search, cancellationToken: cancellationToken.Token);
                return new EngineReponse { Entries = webData.WebPages.TotalEstimatedMatches.Value, Query = search, SearchEngine = this.Name };
            }
            catch (Exception)
            {
                cancellationToken.Cancel();
                throw;
            }

        }
    }

}

