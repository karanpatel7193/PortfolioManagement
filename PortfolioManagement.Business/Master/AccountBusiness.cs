using CommonLibrary;
using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Master;
using PortfolioManagement.Repository.Master;
using System.Data;

namespace PortfolioManagement.Business.Master
{
    public class AccountBusiness : CommonBusiness,IAccounRepository
    {
        ISql sql;
        public AccountBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }

        /// This function maps reader table fields to the Account entity.
        public AccountEntity MapData(IDataReader reader)
        {
            AccountEntity accountEntity = new AccountEntity();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "Id":
                        accountEntity.Id = MyConvert.ToByte(reader["Id"]);
                        break;
                    case "Name":
                        accountEntity.Name = MyConvert.ToString(reader["Name"]);
                        break;
                    case "PmsId":
                        accountEntity.PmsId = MyConvert.ToInt(reader["PmsId"]);
                        break;
                    
                }
            }
            return accountEntity;
        }

        /// Selects a record by Id.
        public async Task<AccountEntity>SelectForRecord(int id)
        {
            sql.AddParameter("Id", id);
            // Using ExecuteRecordAsync to fetch a single record
            return await sql.ExecuteRecordAsync<AccountEntity>("Account_SelectForRecord", CommandType.StoredProcedure);
        }

        public async Task<List<AccountMainEntity>> SelectForLOV(AccountParameterEntity accountParameterEntity)
        {
            sql.AddParameter("PmsId", accountParameterEntity.PmsId);
            return await sql.ExecuteListAsync<AccountMainEntity>("Account_SelectForLOV", CommandType.StoredProcedure);
        }

        public async Task<AccountGridEntity> SelectForGrid(AccountParameterEntity accountParameterEntity)
        {
            AccountGridEntity accountGridEntity = new AccountGridEntity();

            if (!string.IsNullOrEmpty(accountParameterEntity.Name))
            {
                sql.AddParameter("Name", accountParameterEntity.Name);
            }
            sql.AddParameter("SortExpression", accountParameterEntity.SortExpression);
            sql.AddParameter("SortDirection", accountParameterEntity.SortDirection);
            sql.AddParameter("PageIndex", accountParameterEntity.PageIndex);
            sql.AddParameter("PageSize", accountParameterEntity.PageSize);
            if(accountParameterEntity.PmsId != 0)
            sql.AddParameter("PmsId", accountParameterEntity.PmsId);


            await sql.ExecuteEnumerableMultipleAsync<AccountGridEntity>("Account_SelectForGrid", CommandType.StoredProcedure, 2, accountGridEntity, MapGridEntity);
            return accountGridEntity;
        }
        /// Maps data for Account grid entity.
        public async Task MapGridEntity(int resultSet, AccountGridEntity accountGridEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    accountGridEntity.Accounts.Add(await sql.MapDataDynamicallyAsync<AccountEntity>(reader));
                    break;
                case 1:
                    accountGridEntity.TotalRecords = MyConvert.ToInt(reader["TotalRecords"]);
                    break;
            }
        }
        public async Task<AccountAddEntity> SelectForAdd(AccountParameterEntity accountParameterEntity)
        {
            AccountAddEntity accountAddEntity = new AccountAddEntity();

            sql.AddParameter("Id", accountParameterEntity.Id);
            sql.AddParameter("PmsId", accountParameterEntity.PmsId);

            await sql.ExecuteEnumerableMultipleAsync<AccountAddEntity>(
                "Account_SelectForAdd", CommandType.StoredProcedure, 1, accountAddEntity, MapAddEntity);
            return accountAddEntity;
        }
        public async Task MapAddEntity(int resultSet, AccountAddEntity accountAddEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    accountAddEntity.Brockers.Add(await sql.MapDataDynamicallyAsync<AccountBrokerSelectEntity>(reader));
                    break;
            }
        }
        public async Task<AccountEditEntity> SelectForEdit(AccountParameterEntity accountParameterEntity)
        {
            AccountEditEntity accountEditEntity = new AccountEditEntity();

            sql.AddParameter("Id", accountParameterEntity.Id);
            sql.AddParameter("PmsId", accountParameterEntity.PmsId);

            await sql.ExecuteEnumerableMultipleAsync<AccountEditEntity>(
                "Account_SelectForEdit", CommandType.StoredProcedure, 2, accountEditEntity, MapEditEntity);
            return accountEditEntity;
        }
        /// Maps data for Account grid entity.
        public async Task MapEditEntity(int resultSet, AccountEditEntity accountEditEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 1:
                    accountEditEntity.Account.Brokers.Add(await sql.MapDataDynamicallyAsync<AccountBrokerSelectEntity>(reader));
                    break;
                case 0:
                    accountEditEntity.Account = await sql.MapDataDynamicallyAsync<AccountEntity>(reader);
                    break;
            }
        }

        public async Task<int> Insert(AccountEntity accountEntity)
        {
            sql.AddParameter("Name", accountEntity.Name);
            sql.AddParameter("BrokerIds", accountEntity.Brokers.ToXML());
            sql.AddParameter("PmsId", accountEntity.PmsId);
            var result = await sql.ExecuteScalarAsync("Account_Insert", CommandType.StoredProcedure);
            return MyConvert.ToInt(result);
        }

        /// Updates an existing record.
        public async Task<int> Update(AccountEntity accountEntity)
        {
            sql.AddParameter("Id", accountEntity.Id);
            sql.AddParameter("Name", accountEntity.Name);
            sql.AddParameter("BrokerIds", accountEntity.Brokers.ToXML());
            return MyConvert.ToInt(await sql.ExecuteScalarAsync("Account_Update", CommandType.StoredProcedure));
        }
        
        /// Deletes a record by Id.
        public async Task Delete(int id)
        {
            sql.AddParameter("Id", id);
            await sql.ExecuteNonQueryAsync("Account_Delete", CommandType.StoredProcedure);
        }
    }
}
