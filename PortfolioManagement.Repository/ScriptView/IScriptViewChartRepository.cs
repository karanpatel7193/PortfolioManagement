using PortfolioManagement.Entity.ScriptView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.ScriptView
{
    public interface IScriptViewChartRepository
    {
        public  Task<ScriptViewChartEntity> SelectForChart(ScriptViewParameterEntity scriptViewParameterEntity);

    }
}
