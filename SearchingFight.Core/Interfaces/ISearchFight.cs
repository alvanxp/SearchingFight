using SearchingFight.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchingFight.Core.Interfaces
{
    public interface ISearchFight
    {
        Task<FightResult> Fight(IEnumerable<string> search);
    }
}
