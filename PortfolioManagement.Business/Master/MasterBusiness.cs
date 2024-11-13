using CommonLibrary;
using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Master;
using PortfolioManagement.Entity.Transaction.StockTransaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.Master
{
    public class MasterBusiness : CommonBusiness
    {
        ISql sql;
        public MasterBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }

        public MasterEntity MapData(IDataReader reader)
        {
            MasterEntity masterEntity = new MasterEntity();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "Id":
                        masterEntity.Id = MyConvert.ToByte(reader["Id"]);
                        break;
                    case "Type":
                        masterEntity.Type = MyConvert.ToString(reader["Type"]);
                        break;
                }
            }
            return masterEntity;
        }

        public async Task<int> Insert(MasterEntity masterEntity)
        {
            sql.AddParameter("Id", masterEntity.Id);
            sql.AddParameter("Type", masterEntity.Type);
            sql.AddParameter("MasterValues", masterEntity.MasterValues.ToXML());
            var result = await sql.ExecuteScalarAsync("Master_Insert", CommandType.StoredProcedure);
            return MyConvert.ToInt(result);
        }

        public async Task<int> Update(MasterEntity masterEntity)
        {
            sql.AddParameter("Id", masterEntity.Id);
            sql.AddParameter("Type", masterEntity.Type);
            sql.AddParameter("MasterValues", masterEntity.MasterValues.ToXML());
            var result = await sql.ExecuteScalarAsync("Master_Update", CommandType.StoredProcedure);
            return MyConvert.ToInt(result);
        }

        public async Task Delete(int id)
        {
            sql.AddParameter("Id", id);
            await sql.ExecuteNonQueryAsync("Master_Delete", CommandType.StoredProcedure);
        }

        public async Task<List<MasterEntity>> SelectForGrid()
        {
            return await sql.ExecuteListAsync<MasterEntity>("Master_SelectForGrid", CommandType.StoredProcedure);
        }

        public async Task<MasterEntity> SelectForRecord(short Id)
        {
            MasterEntity masterEntity = new MasterEntity();
            sql.AddParameter("Id", Id);
            await sql.ExecuteEnumerableMultipleAsync<MasterEntity>("Master_SelectForRecord", CommandType.StoredProcedure, 2, masterEntity, MapRecordEntity);
            return masterEntity;
        }

        private async Task MapRecordEntity(int resultSet, MasterEntity masterEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    var entity = await sql.MapDataDynamicallyAsync<MasterEntity>(reader);
                    masterEntity.Id = entity.Id;
                    masterEntity.Type = entity.Type;
                    break;
                case 1:
                    masterEntity.MasterValues.Add(await sql.MapDataDynamicallyAsync<MasterValueEntity>(reader));
                    break;
            }
        }
    }
}
