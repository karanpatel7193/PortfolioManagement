using System.Collections.Generic;

namespace PortfolioManagement.Entity.Transaction
{
    public class ScrapNifty50DataEntity
    {
        public IndexEntity IndexEntity = new IndexEntity();
        public List<ScriptDaySummaryEntity> ScriptDaySummaryEntities = new List<ScriptDaySummaryEntity>();
    }
}
