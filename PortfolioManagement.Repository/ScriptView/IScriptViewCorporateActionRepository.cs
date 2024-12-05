using PortfolioManagement.Entity.ScriptView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.ScriptView
{
    public interface IScriptViewCorporateActionRepository
    {
        public  Task<List<ScriptViewCorporateActionBonusEntity>> SelectForBonus(int id);
        public  Task<List<ScriptViewCorporateActionSplitEntity>> SelectForSplit(int id);
    }
}
