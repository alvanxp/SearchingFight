using System.Collections.Generic;

namespace SearchingFight.Core.Entities
{
    public class FightResult
    {
        public FightResult()
        {
            Winners = new List<EngineReponse>();
            QueryResults = new List<QueryResult>();
        }
        public string TotalWinner { get; set; }
        public List<EngineReponse> Winners { get; set; }
        public List<QueryResult> QueryResults { get; set; }
    }
}