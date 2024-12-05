using CommonLibrary;
using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Master;
using PortfolioManagement.Repository.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.Master
{
    public class BrokerBusiness : CommonBusiness, IBrokerRepository
    {
        ISql sql;
        public BrokerBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }
        /// This function maps reader table fields to the Broker entity.
        public BrokerEntity MapData(IDataReader reader)
        {
            BrokerEntity brokerEntity = new BrokerEntity();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i)) {
                
                    case "Id":
                        brokerEntity.Id = MyConvert.ToByte(reader["Id"]);
                        break;
                    case "Name":
                        brokerEntity.Name = MyConvert.ToString(reader["Name"]);
                        break;
                    case "BrokerTypeId":
                        brokerEntity.BrokerTypeId = MyConvert.ToByte(reader["BrokerTypeId"]);
                        break;
                    case "BuyBrokerage":
                        brokerEntity.BuyBrokerage = MyConvert.ToDouble(reader["BuyBrokerage"]);
                        break;
                    case "SellBrokerage":
                        brokerEntity.SellBrokerage = MyConvert.ToDouble(reader["SellBrokerage"]);
                        break;
                    case "PmsId":
                        brokerEntity.PmsId = MyConvert.ToInt(reader["PmsId"]);
                        break;
                }
            }
            return brokerEntity;
        }

        /// SelectForRecord

        public async Task<BrokerEntity> SelectForRecord(int id)
        {
            sql.AddParameter("Id", id);
            return await sql.ExecuteRecordAsync<BrokerEntity>("Broker_SelectForRecord", CommandType.StoredProcedure);
        }

        ///  Broker LOV

        public async Task<List<BrokerMainEntity>> SelectForLOV(BrokerParameterEntity brokerParameterEntity)
        {
            sql.AddParameter("PmsId", brokerParameterEntity.PmsId);
            return await sql.ExecuteListAsync<BrokerMainEntity>("Broker_SelectForLov", CommandType.StoredProcedure);
        }

        ///  page grid data.

        public async Task<BrokerGridEntity> SelectForGrid(BrokerParameterEntity brokerParameterEntity)
        {
            BrokerGridEntity brokerGridEntity = new BrokerGridEntity();

            if (brokerParameterEntity.Name != String.Empty)
            {
                sql.AddParameter("Name", brokerParameterEntity.Name);
            }
            sql.AddParameter("SortExpression", brokerParameterEntity.SortExpression);
            sql.AddParameter("SortDirection", brokerParameterEntity.SortDirection);
            sql.AddParameter("PageIndex", brokerParameterEntity.PageIndex);
            sql.AddParameter("PageSize", brokerParameterEntity.PageSize);
            sql.AddParameter("PmsId", brokerParameterEntity.PmsId);
            if (brokerParameterEntity.BrokerTypeId != 0)
                sql.AddParameter("BrokerTypeId", brokerParameterEntity.BrokerTypeId);


            await sql.ExecuteEnumerableMultipleAsync<BrokerGridEntity>
                ("Broker_SelectForGrid", CommandType.StoredProcedure, 2, brokerGridEntity, MapGridEntity);
            return brokerGridEntity;
        }

        public async Task MapGridEntity(int resultSet, BrokerGridEntity brokerGridEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    brokerGridEntity.Brokers.Add(await sql.MapDataDynamicallyAsync<BrokerEntity>(reader));
                    break;
                case 1:
                    brokerGridEntity.TotalRecords = MyConvert.ToInt(reader["TotalRecords"]);
                    break;
            }
        }

        /// This function inserts a record into the Broker table.
        public async Task<int> Insert(BrokerEntity brokerEntity)
        {
            sql.AddParameter("Name", brokerEntity.Name);
            sql.AddParameter("BrokerTypeId", brokerEntity.BrokerTypeId);
            sql.AddParameter("BuyBrokerage", brokerEntity.BuyBrokerage);
            sql.AddParameter("SellBrokerage", brokerEntity.SellBrokerage);
            sql.AddParameter("PmsId", brokerEntity.PmsId);
            var r = await sql.ExecuteScalarAsync("Broker_Insert", CommandType.StoredProcedure);
            return MyConvert.ToInt(r);
            //return MyConvert.ToInt(await sql.ExecuteScalarAsync("Broker_Insert", CommandType.StoredProcedure));
        }

        /// This function updates a record in the Broker table.
        public async Task<int> Update(BrokerEntity brokerEntity)
        {
            sql.AddParameter("Id", brokerEntity.Id);
            sql.AddParameter("Name", brokerEntity.Name);
            sql.AddParameter("BrokerTypeId", brokerEntity.BrokerTypeId);
            sql.AddParameter("BuyBrokerage", brokerEntity.BuyBrokerage);
            sql.AddParameter("SellBrokerage", brokerEntity.SellBrokerage);
            return MyConvert.ToInt(await sql.ExecuteScalarAsync("Broker_Update", CommandType.StoredProcedure));
        }

        /// <summary>
        /// This function deletes a record from the Broker table.
        /// </summary>
        public async Task Delete(int id)
        {
            sql.AddParameter("BrokerId", id);
            await sql.ExecuteNonQueryAsync("Broker_Delete", CommandType.StoredProcedure);
        }
    }
}

