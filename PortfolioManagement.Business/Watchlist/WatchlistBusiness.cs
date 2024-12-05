using CommonLibrary.SqlDB;
using CommonLibrary;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortfolioManagement.Entity.Watchlist;
using PortfolioManagement.Entity.Master;
using PortfolioManagement.Entity.Transaction.StockTransaction;
using PortfolioManagement.Entity.Transaction;
using PortfolioManagement.Entity.Account;
using PortfolioManagement.Repository.Watchlist;

namespace PortfolioManagement.Business.Watchlist
{
    public class WatchlistBusiness : CommonBusiness, IWatchlistRepository
    {
        ISql sql;

        public WatchlistBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }

        /// <summary>
        /// Maps the Watchlist table fields to the Watchlist entity.
        /// </summary>
        public WatchlistEntity MapData(IDataReader reader)
        {
            WatchlistEntity watchlistEntity = new WatchlistEntity();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "Id":
                        watchlistEntity.Id = MyConvert.ToInt(reader["Id"]);
                        break;
                    case "PmsId":
                        watchlistEntity.PmsId = MyConvert.ToInt(reader["PmsId"]);
                        break;
                    case "Name":
                        watchlistEntity.Name = MyConvert.ToString(reader["Name"]);
                        break;
                }
            }
            return watchlistEntity;
        }
        public async Task<List<WatchlistMainEntity>> SelectForTab(int PmsId)
        {
            sql.AddParameter("PmsId", PmsId);  
            return await sql.ExecuteListAsync<WatchlistMainEntity>("Watchlist_SelectForTab", CommandType.StoredProcedure);
        }

        public async Task<List<WatchlistScriptTabEntity>> SelectForTabScript(WatchlistParameterEntity watchlistParameterEntity)
        {
            sql.AddParameter("PmsId", watchlistParameterEntity.PmsId);
            sql.AddParameter("WatchListId", watchlistParameterEntity.WatchListId);
            return await sql.ExecuteListAsync<WatchlistScriptTabEntity>("Watchlist_SelectForTabScript", CommandType.StoredProcedure);
        }

        public async Task<int> Insert(WatchlistEntity watchlistEntity)
        {
            sql.AddParameter("Name", watchlistEntity.Name);
            sql.AddParameter("PmsId", watchlistEntity.PmsId);
            var result = await sql.ExecuteScalarAsync("Watchlist_Insert", CommandType.StoredProcedure);
            return MyConvert.ToInt(result);
        }

        public async Task<int> InsertScript(WatchlistParameterEntity watchlistParameterEntity)
        {
            sql.AddParameter("WatchListId", watchlistParameterEntity.WatchListId);
            sql.AddParameter("ScriptId", watchlistParameterEntity.ScriptId);
            sql.AddParameter("PmsId", watchlistParameterEntity.PmsId);

            return MyConvert.ToInt(await sql.ExecuteScalarAsync("WatchlistScript_Insert", CommandType.StoredProcedure));
        }

        public async Task<int> Update(WatchlistEntity watchlistEntity)
        {
            sql.AddParameter("Id", watchlistEntity.Id);
            sql.AddParameter("Name", watchlistEntity.Name);
            return MyConvert.ToInt(await sql.ExecuteScalarAsync("Watchlist_Update", CommandType.StoredProcedure));
        }

        public async Task Delete(int id)
        {
            sql.AddParameter("Id", id);
            await sql.ExecuteNonQueryAsync("Watchlist_Delete", CommandType.StoredProcedure);
        }

        public async Task DeleteScript (int id)
        {
            sql.AddParameter("Id", id);
            await sql.ExecuteNonQueryAsync("WatchlistScript_Delete", CommandType.StoredProcedure);
        }
    }
}
