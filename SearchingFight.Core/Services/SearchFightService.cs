using SearchingFight.Core.Entities;
using SearchingFight.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SearchingFight.Core.Services
{
    public class SearchFightService : ISearchFight
    {
        private readonly IEnumerable<ISearchEngine> _searchEngines;

        public SearchFightService(IEnumerable<ISearchEngine> searchEngines)
        {
            _searchEngines = searchEngines;
        }
        public async Task<FightResult> Fight(IEnumerable<string> queries)
        {
            if (queries.Count() < 2) throw new InvalidOperationException("Should be more than 1 element to compare");
            var result = new FightResult();
            foreach (var query in queries)
            {
                var queryResult = await FindEntries(query);
                result.QueryResults.Add(queryResult);
            }
            result.Winners = GetWinners(result.QueryResults);
            var totalWinner = result.Winners.OrderByDescending(_ => _.Entries).FirstOrDefault();
            result.TotalWinner = totalWinner?.Query;
            return result;
        }

        private List<EngineReponse> GetWinners(List<QueryResult> queryResults)
        {
            var winnersByEngine = new Dictionary<string, EngineReponse>();
            foreach (var queryResult in queryResults)
            {
                foreach (var engineResponse in queryResult.EnginesResponses)
                {
                    if (winnersByEngine.TryGetValue(engineResponse.SearchEngine, out var maxByEngine))
                    {
                        if (engineResponse.Entries > maxByEngine.Entries)
                            winnersByEngine[engineResponse.SearchEngine] = engineResponse;
                    }
                    else
                        winnersByEngine.Add(engineResponse.SearchEngine, engineResponse);
                }
            }

            return winnersByEngine.Values.ToList();
        }

        private async Task<QueryResult> FindEntries(string query)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var result = new QueryResult();
            result.Query = query;
            var tasks = new List<Task<EngineReponse>>();
            foreach (var engine in _searchEngines)
            {
                tasks.Add(engine.FindEntries(query, cancellationTokenSource));
            }

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                var response = task.Result;
                response.Query = query;
                result.TotalEntries += response.Entries;
                result.EnginesResponses.Add(response);
            }

            return result;
        }
    }
}
