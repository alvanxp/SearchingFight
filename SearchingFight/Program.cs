using Microsoft.Extensions.DependencyInjection;
using SearchingFight.Core.Interfaces;
using SearchingFight.Core.Services;
using SearchingFight.Infrastructure;
using System;
using System.Threading.Tasks;

namespace SearchingFight
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var collection = new ServiceCollection();
                collection.AddHttpClient();
                collection.AddLogging();
                collection.AddScoped<ISearchEngine, GoogleSearch>();
                collection.AddScoped<ISearchEngine, BingSearch>();
                var serviceProvider = collection.BuildServiceProvider();
                var services = serviceProvider.GetServices<ISearchEngine>();

                var service = new SearchFightService(services);
                var result = await service.Fight(args);
                foreach (var queryResult in result.QueryResults)
                {
                    Console.Write($"{queryResult.Query}: ");
                    foreach (var response in queryResult.EnginesResponses)
                    {
                        Console.Write($" {response.SearchEngine}: {response.Entries}");
                    }
                    Console.WriteLine();
                }

                foreach (var winner in result.Winners)
                {
                    Console.WriteLine($"{winner.SearchEngine} winner: {winner.Query}");
                }

                Console.WriteLine($"Total Winner : {result.TotalWinner}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
