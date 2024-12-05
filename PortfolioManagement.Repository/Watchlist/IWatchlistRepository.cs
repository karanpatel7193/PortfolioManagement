using PortfolioManagement.Entity.Watchlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.Watchlist
{
    public interface IWatchlistRepository
    {
        public  Task<List<WatchlistMainEntity>> SelectForTab(int PmsId);
        public  Task<List<WatchlistScriptTabEntity>> SelectForTabScript(WatchlistParameterEntity watchlistParameterEntity);
        public  Task<int> Insert(WatchlistEntity watchlistEntity);
        public  Task<int> InsertScript(WatchlistParameterEntity watchlistParameterEntity);
        public  Task<int> Update(WatchlistEntity watchlistEntity);
        public  Task Delete(int id);
        public  Task DeleteScript(int id);
    }
}
