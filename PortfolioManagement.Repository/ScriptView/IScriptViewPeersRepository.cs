using PortfolioManagement.Entity.ScriptView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.ScriptView
{
    public interface IScriptViewPeersRepository
    {
        public Task<List<ScriptViewPeersEntity>> SelectForPeers(int id);

    }
}
