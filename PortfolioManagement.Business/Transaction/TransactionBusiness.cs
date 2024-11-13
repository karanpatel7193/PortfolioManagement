using CommonLibrary;
using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Master;
using PortfolioManagement.Entity.Transaction;
using System;
using System.Data;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.Transaction;

/// <summary>
/// This class having crud operation function of table Transaction
/// Created By :: Rekansh Patel
/// Created On :: 10/30/2020
/// </summary>
public class TransactionBusiness : CommonBusiness
    {
        ISql sql;

        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public TransactionBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This function return all columns values for particular Transaction record
        /// </summary>
        /// <param name="Id">Particular Record</param>
        /// <returns>Entity</returns>
        public async Task<TransactionEntity> SelectForRecord(long id)
        {
            sql.AddParameter("Id", id);
            return await sql.ExecuteRecordAsync<TransactionEntity>("Transaction_SelectForRecord", CommandType.StoredProcedure);
        }

        /// <summary>
        /// This function return all LOVs data for Transaction page add mode
        /// </summary>
        /// <param name="transactionParameterEntity">Parameter</param>
        /// <returns>Add modes all LOVs data</returns>
        public async Task<TransactionAddEntity> SelectForAdd(TransactionParameterEntity transactionParameterEntity)
        {
            TransactionAddEntity transactionAddEntity = new TransactionAddEntity();
            await sql.ExecuteEnumerableMultipleAsync<TransactionAddEntity>("Transaction_SelectForAdd", CommandType.StoredProcedure, 4, transactionAddEntity, MapAddEntity);
            return transactionAddEntity;
        }

        /// <summary>
        /// This function map data for Transaction page add mode LOVs
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="transactionAddEntity">Add mode Entity for fill data</param>
        /// <param name="reader">Database reader</param>
        public async Task MapAddEntity(int resultSet, TransactionAddEntity transactionAddEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    transactionAddEntity.Accounts.Add(await sql.MapDataDynamicallyAsync<AccountMainEntity>(reader));
                    break;
                case 1:
                    transactionAddEntity.Types.Add(await sql.MapDataDynamicallyAsync<TypeMainEntity>(reader));
                    break;
                case 2:
                    transactionAddEntity.Scripts.Add(await sql.MapDataDynamicallyAsync<ScriptMainEntity>(reader));
                    break;
                case 3:
                    transactionAddEntity.Investers.Add(await sql.MapDataDynamicallyAsync<InvesterMainEntity>(reader));
                    break;

            }
        }

        /// <summary>
        /// This function return all LOVs data and edit record information for Transaction page edit mode
        /// </summary>
        /// <param name="transactionParameterEntity">Parameter</param>
        /// <returns>Edit modes all LOVs data and edit record information</returns>
        public async Task<TransactionEditEntity> SelectForEdit(TransactionParameterEntity transactionParameterEntity)
        {
            TransactionEditEntity transactionEditEntity = new TransactionEditEntity();
            sql.AddParameter("Id", transactionParameterEntity.Id);
            await sql.ExecuteEnumerableMultipleAsync<TransactionEditEntity>("Transaction_SelectForEdit", CommandType.StoredProcedure, 5, transactionEditEntity, MapEditEntity);
            return transactionEditEntity;
        }

        /// <summary>
        /// This function map data for Transaction page edit mode LOVs and edit record information
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="transactionEditEntity">Edit mode Entity for fill data and edit record information</param>
        /// <param name="reader">Database reader</param>
        public async Task MapEditEntity(int resultSet, TransactionEditEntity transactionEditEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    transactionEditEntity.Transaction = await sql.MapDataDynamicallyAsync<TransactionEntity>(reader);
                    break;
                case 1:
                    transactionEditEntity.Accounts.Add(await sql.MapDataDynamicallyAsync<AccountMainEntity>(reader));
                    break;
                case 2:
                    transactionEditEntity.Types.Add(await sql.MapDataDynamicallyAsync<TypeMainEntity>(reader));
                    break;
                case 3:
                    transactionEditEntity.Scripts.Add(await sql.MapDataDynamicallyAsync<ScriptMainEntity>(reader));
                    break;
                case 4:
                    transactionEditEntity.Investers.Add(await sql.MapDataDynamicallyAsync<InvesterMainEntity>(reader));
                    break;

            }
        }

        /// <summary>
        /// This function returns Transaction list page grid data.
        /// </summary>
        /// <param name="transactionParameterEntity">Filter paramters</param>
        /// <returns>Transaction grid data</returns>
        public async Task<TransactionGridEntity> SelectForGrid(TransactionParameterEntity transactionParameterEntity)
        {
            TransactionGridEntity transactionGridEntity = new TransactionGridEntity();
            if (transactionParameterEntity.Date != DateTime.MinValue)
                sql.AddParameter("Date", transactionParameterEntity.Date);
            if (transactionParameterEntity.AccountId != 0)
                sql.AddParameter("AccountId", transactionParameterEntity.AccountId);
            if (transactionParameterEntity.TypeId != 0)
                sql.AddParameter("TypeId", transactionParameterEntity.TypeId);
            if (transactionParameterEntity.ScriptId != 0)
                sql.AddParameter("ScriptId", transactionParameterEntity.ScriptId);
            if (transactionParameterEntity.InvesterId != 0)
                sql.AddParameter("InvesterId", transactionParameterEntity.InvesterId);

            sql.AddParameter("SortExpression", transactionParameterEntity.SortExpression);
            sql.AddParameter("SortDirection", transactionParameterEntity.SortDirection);
            sql.AddParameter("PageIndex", transactionParameterEntity.PageIndex);
            sql.AddParameter("PageSize", transactionParameterEntity.PageSize);
            await sql.ExecuteEnumerableMultipleAsync<TransactionGridEntity>("Transaction_SelectForGrid", CommandType.StoredProcedure, 2, transactionGridEntity, MapGridEntity);
            return transactionGridEntity;
        }

        /// <summary>
        /// This function map data for Transaction grid data
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="transactionGridEntity">Transaction grid data</param>
        /// <param name="reader">Database reader</param>
        public async Task MapGridEntity(int resultSet, TransactionGridEntity transactionGridEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    transactionGridEntity.Transactions.Add(await sql.MapDataDynamicallyAsync<TransactionEntity>(reader));
                    break;
                case 1:
                    transactionGridEntity.TotalRecords = MyConvert.ToInt(reader["TotalRecords"]);
                    break;
            }
        }

        /// <summary>
        /// This function returns Transaction list page grid data and LOV data
        /// </summary>
        /// <param name="transactionParameterEntity">Filter paramters</param>
        /// <returns>Transaction grid data and LOV data</returns>
        public async Task<TransactionListEntity> SelectForList(TransactionParameterEntity transactionParameterEntity)
        {
            TransactionListEntity transactionListEntity = new TransactionListEntity();
            if (transactionParameterEntity.Date != DateTime.MinValue)
                sql.AddParameter("Date", transactionParameterEntity.Date);
            if (transactionParameterEntity.AccountId != 0)
                sql.AddParameter("AccountId", transactionParameterEntity.AccountId);
            if (transactionParameterEntity.TypeId != 0)
                sql.AddParameter("TypeId", transactionParameterEntity.TypeId);
            if (transactionParameterEntity.ScriptId != 0)
                sql.AddParameter("ScriptId", transactionParameterEntity.ScriptId);
            if (transactionParameterEntity.InvesterId != 0)
                sql.AddParameter("InvesterId", transactionParameterEntity.InvesterId);

            sql.AddParameter("SortExpression", transactionParameterEntity.SortExpression);
            sql.AddParameter("SortDirection", transactionParameterEntity.SortDirection);
            sql.AddParameter("PageIndex", transactionParameterEntity.PageIndex);
            sql.AddParameter("PageSize", transactionParameterEntity.PageSize);
            await sql.ExecuteEnumerableMultipleAsync<TransactionListEntity>("Transaction_SelectForList", CommandType.StoredProcedure, 6, transactionListEntity, MapListEntity);
            return transactionListEntity;
        }

        /// <summary>
        /// This function map data for Transaction list page grid data and LOV data
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="transactionListEntity">Transaction list page grid data and LOV data</param>
        /// <param name="reader">Database reader</param>
        public async Task MapListEntity(int resultSet, TransactionListEntity transactionListEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    transactionListEntity.Accounts.Add(await sql.MapDataDynamicallyAsync<AccountMainEntity>(reader));
                    break;
                case 1:
                    transactionListEntity.Types.Add(await sql.MapDataDynamicallyAsync<TypeMainEntity>(reader));
                    break;
                case 2:
                    transactionListEntity.Scripts.Add(await sql.MapDataDynamicallyAsync<ScriptMainEntity>(reader));
                    break;
                case 3:
                    transactionListEntity.Investers.Add(await sql.MapDataDynamicallyAsync<InvesterMainEntity>(reader));
                    break;

                case 4:
                    transactionListEntity.Transactions.Add(await sql.MapDataDynamicallyAsync<TransactionEntity>(reader));
                    break;
                case 5:
                    transactionListEntity.TotalRecords = MyConvert.ToInt(reader["TotalRecords"]);
                    break;
            }
        }

        /// <summary>
        /// This function insert record in Transaction table.
        /// </summary>
        /// <returns>Identity / AlreadyExist = 0</returns>
        public async Task<long> Insert(TransactionEntity transactionEntity)
        {
            sql.AddParameter("Date", transactionEntity.Date);
            sql.AddParameter("AccountId", transactionEntity.AccountId);
            sql.AddParameter("TypeId", transactionEntity.TypeId);
            if (transactionEntity.ScriptId != 0)
                sql.AddParameter("ScriptId", transactionEntity.ScriptId);
            if (transactionEntity.Qty != 0)
                sql.AddParameter("Qty", transactionEntity.Qty);
            if (transactionEntity.Price != 0)
                sql.AddParameter("Price", transactionEntity.Price);
            sql.AddParameter("Brokerage", transactionEntity.Brokerage);
            sql.AddParameter("Cr", transactionEntity.Cr);
            sql.AddParameter("Dr", transactionEntity.Dr);
            sql.AddParameter("Balance", transactionEntity.Balance);
            sql.AddParameter("Description", transactionEntity.Description);
            if (transactionEntity.InvesterId != 0)
                sql.AddParameter("InvesterId", transactionEntity.InvesterId);
            return MyConvert.ToLong(await sql.ExecuteScalarAsync("Transaction_Insert", CommandType.StoredProcedure));
        }

        /// <summary>
        /// This function update record in Transaction table.
        /// </summary>
        /// <returns>PrimaryKey Field Value / AlreadyExist = 0</returns>
        public async Task<long> Update(TransactionEntity transactionEntity)
        {
            sql.AddParameter("Id", transactionEntity.Id);
            sql.AddParameter("Date", transactionEntity.Date);
            sql.AddParameter("AccountId", transactionEntity.AccountId);
            sql.AddParameter("TypeId", transactionEntity.TypeId);
            if (transactionEntity.ScriptId != 0)
                sql.AddParameter("ScriptId", transactionEntity.ScriptId);
            if (transactionEntity.Qty != 0)
                sql.AddParameter("Qty", transactionEntity.Qty);
            if (transactionEntity.Price != 0)
                sql.AddParameter("Price", transactionEntity.Price);
            if (transactionEntity.Brokerage != 0)
                sql.AddParameter("Brokerage", transactionEntity.Brokerage);
            sql.AddParameter("Cr", transactionEntity.Cr);
            sql.AddParameter("Dr", transactionEntity.Dr);
            sql.AddParameter("Balance", transactionEntity.Balance);
            sql.AddParameter("Description", transactionEntity.Description);
            if (transactionEntity.InvesterId != 0)
                sql.AddParameter("InvesterId", transactionEntity.InvesterId);
            return MyConvert.ToLong(await sql.ExecuteScalarAsync("Transaction_Update", CommandType.StoredProcedure));
        }

        /// <summary>
        /// This function delete record from Transaction table.
        /// </summary>
        public async Task Delete(long id)
        {
            sql.AddParameter("Id", id);
            await sql.ExecuteNonQueryAsync("Transaction_Delete", CommandType.StoredProcedure);
        }

        /// <summary>
        /// This function delete record from Transaction table.
        /// </summary>
        public async Task<double> SelectForBalance(TransactionParameterEntity transactionParameterEntity)
        {
            sql.AddParameter("AccountId", transactionParameterEntity.AccountId);
            if(transactionParameterEntity.Date == DateTime.MinValue)
                sql.AddParameter("DateTime", DateTime.UtcNow);
            else
                sql.AddParameter("DateTime", transactionParameterEntity.Date);
            return MyConvert.ToDouble(await sql.ExecuteScalarAsync("Transaction_SelectForBalance", CommandType.StoredProcedure));
        }
        #endregion
 }

