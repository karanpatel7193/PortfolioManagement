using PortfolioManagement.Entity.ScriptView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.ScriptView
{
    public interface IScriptViewRangeRepository
    {
        public  Task<ScriptViewRangeChartEntity> SelectForRange(int id);
    }
}
