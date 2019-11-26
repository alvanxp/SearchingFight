using SearchingFight.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SearchingFight.Core.Interfaces
{
    public interface ISearchEngine
    {
        string Name { get; }
        Task<EngineReponse> FindEntries(string search, CancellationTokenSource cancellationToken);
    }
}
