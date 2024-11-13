using CommonLibrary.SqlDB;
using CommonLibrary;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Account;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.Account
{
    public class PmsBusiness : CommonBusiness
    {
        ISql sql;
        public PmsBusiness(IConfiguration configuration) : base(configuration)
        {
            sql = CreateSqlInstance();
        }
 
        public PmsEntity MapData(IDataReader reader)
        {
            PmsEntity pmsEntity = new PmsEntity();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "Id":
                        pmsEntity.Id = MyConvert.ToInt(reader["Id"]);
                        break;
                    case "Name":
                        pmsEntity.Name = MyConvert.ToString(reader["Name"]);
                        break;
                    case "IsActive":
                        pmsEntity.IsActive = MyConvert.ToBoolean(reader["IsActive"]);
                        break;
                    case "Type":
                        pmsEntity.Type = MyConvert.ToString(reader["Type"]);
                        break;
                }
            }
            return pmsEntity;
        }

        public async Task<PmsEntity> SelectForRecord(int Id)
        {
            sql.AddParameter("Id", Id);
            return await sql.ExecuteRecordAsync<PmsEntity>("PMS_SelectForRecord", CommandType.StoredProcedure);
        }
        public async Task<PmsGridEntity> SelectForGrid(PmsParameterEntity pmsParameterEntity)
        {
            PmsGridEntity pmsGridEntity = new PmsGridEntity();
            if (pmsParameterEntity.Name != string.Empty)
                sql.AddParameter("Name", pmsParameterEntity.Name);

            sql.AddParameter("SortExpression", pmsParameterEntity.SortExpression);
            sql.AddParameter("SortDirection", pmsParameterEntity.SortDirection);
            sql.AddParameter("PageIndex", pmsParameterEntity.PageIndex);
            sql.AddParameter("PageSize", pmsParameterEntity.PageSize);
            await sql.ExecuteEnumerableMultipleAsync<PmsGridEntity>("PMS_SelectForGrid", CommandType.StoredProcedure, 2, pmsGridEntity, MapGridEntity);
            return pmsGridEntity;
        }

        public async Task MapGridEntity(int resultSet, PmsGridEntity pmsGridEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    pmsGridEntity.pmss.Add(await sql.MapDataDynamicallyAsync<PmsEntity>(reader));
                    break;
                case 1:
                    pmsGridEntity.TotalRecords = MyConvert.ToInt(reader["TotalRecords"]);
                    break;
            }
        }
     
        public async Task<int> Insert(PmsEntity pmsEntity)
        {
            sql.AddParameter("Name", pmsEntity.Name);
            sql.AddParameter("IsActive", pmsEntity.IsActive);
            sql.AddParameter("Type", pmsEntity.Type);
            return MyConvert.ToInt(await sql.ExecuteScalarAsync("PMS_Insert", CommandType.StoredProcedure));
        }
        public async Task<int> Update(PmsEntity pmsEntity)
        {
            sql.AddParameter("Id", pmsEntity.Id);
            sql.AddParameter("Name", pmsEntity.Name);
            sql.AddParameter("IsActive", pmsEntity.IsActive);
            sql.AddParameter("Type", pmsEntity.Type);
            return MyConvert.ToInt(await sql.ExecuteScalarAsync("PMS_Update", CommandType.StoredProcedure));
        }

        public async Task Delete(int Id)
        {
            sql.AddParameter("Id", Id);
            await sql.ExecuteNonQueryAsync("PMS_Delete", CommandType.StoredProcedure);
        }
    }
}
