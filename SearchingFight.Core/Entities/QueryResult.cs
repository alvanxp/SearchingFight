using System.Collections.Generic;

namespace SearchingFight.Core.Entities
{
    public class QueryResult
    {
        public QueryResult()
        {
            EnginesResponses = new List<EngineReponse>();
        }
        public List<EngineReponse> EnginesResponses { get; set; }
        public string Query { get; set; }
        public long TotalEntries { get; set; }
    }
}