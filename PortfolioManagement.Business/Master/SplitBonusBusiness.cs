using CommonLibrary.SqlDB;
using CommonLibrary;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.Master
{
    //internal class SplitBonusBusiness
    //{
    //}
    public class SplitBonusBusiness : CommonBusiness
    {
        ISql sql;

        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public SplitBonusBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This function return map reader table field to Entity of SplitBonus.
        /// </summary>
        /// <returns>Entity</returns>
        public SplitBonusEntity MapData(IDataReader reader)
        {
            SplitBonusEntity splitBonusEntity = new SplitBonusEntity();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "Id":
                        splitBonusEntity.Id = MyConvert.ToInt(reader["Id"]);
                        break;
                    case "NseCode":
                        splitBonusEntity.NseCode = MyConvert.ToString(reader["NseCode"]);
                        break;
                    case "IsSplit":
                        splitBonusEntity.IsSplit = MyConvert.ToBoolean(reader["IsSplit"]);
                        break;
                    case "IsBonus":
                        splitBonusEntity.IsBonus = MyConvert.ToBoolean(reader["IsBonus"]);
                        break;
                    case "OldFaceValue":
                        splitBonusEntity.OldFaceValue = MyConvert.ToDouble(reader["OldFaceValue"]);
                        break;
                    case "NewFaceValue":
                        splitBonusEntity.NewFaceValue = MyConvert.ToDouble(reader["NewFaceValue"]);
                        break;
                    case "FromRatio":
                        splitBonusEntity.FromRatio = MyConvert.ToInt(reader["FromRatio"]);
                        break;
                    case "ToRatio":
                        splitBonusEntity.ToRatio = MyConvert.ToInt(reader["ToRatio"]);
                        break;
                    case "AnnounceDate":
                        splitBonusEntity.AnnounceDate = MyConvert.ToDateTime(reader["AnnounceDate"]);
                        break;
                    case "RewardDate":
                        splitBonusEntity.RewardDate = MyConvert.ToDateTime(reader["RewardDate"]);
                        break;
                }
            }
            return splitBonusEntity;
        }

        /// <summary>
        /// This function return all columns values for particular SplitBonus record
        /// </summary>
        /// <param name="Id">Particular Record</param>
        /// <returns>Entity</returns>
        public async Task<SplitBonusEntity> SelectForRecord(int id)
        {
            sql.AddParameter("Id", id);
            return await sql.ExecuteRecordAsync<SplitBonusEntity>("SplitBonus_SelectForRecord", CommandType.StoredProcedure);
        }

        /// <summary>
        /// This function returns main informations for bind SplitBonus LOV
        /// </summary>
        /// <param name="SplitBonusEntity">Filter criteria by Entity</param>
        /// <returns>Main Entitys</returns>
		//public async Task<List<SplitBonusMainEntity>> SelectForLOV(SplitBonusParameterEntity SplitBonusParameterEntity)
  //      {
  //          return await sql.ExecuteListAsync<SplitBonusMainEntity>("SplitBonus_SelectForLOV", CommandType.StoredProcedure);
  //      }

        /// <summary>
        /// This function returns SplitBonus list page grid data.
        /// </summary>
        /// <param name="SplitBonusParameterEntity">Filter paramters</param>
        /// <returns>SplitBonus grid data</returns>
        public async Task<SplitBonusGridEntity> SelectForGrid()
        {
            SplitBonusGridEntity splitBonusGridEntity = new SplitBonusGridEntity();
            await sql.ExecuteEnumerableMultipleAsync<SplitBonusGridEntity>("SplitBonus_SelectForGrid", CommandType.StoredProcedure, 2, splitBonusGridEntity, MapGridEntity);
            return splitBonusGridEntity;

        }

        /// <summary>
        /// This function map data for SplitBonus grid data
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="SplitBonusGridEntity">SplitBonus grid data</param>
        /// <param name="reader">Database reader</param>
        public async Task MapGridEntity(int resultSet, SplitBonusGridEntity splitBonusGridEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    splitBonusGridEntity.SplitBonuss.Add(await sql.MapDataDynamicallyAsync<SplitBonusEntity>(reader));
                    break;
                case 1:
                    splitBonusGridEntity.TotalRecords = MyConvert.ToInt(reader["TotalRecords"]);
                    break;
            }
        }

        /// <summary>
        /// This function insert record in SplitBonus table.
        /// </summary>
        /// <returns>Identity / AlreadyExist = 0</returns>
        public async Task<int> Insert(SplitBonusEntity splitBonusEntity)
        {
            //if you insert zero in ui it will shows null in database
            if (splitBonusEntity.OldFaceValue != 0)
                sql.AddParameter("OldFaceValue", splitBonusEntity.OldFaceValue);
            if (splitBonusEntity.NewFaceValue != 0)
                sql.AddParameter("NewFaceValue", splitBonusEntity.NewFaceValue);
            if (splitBonusEntity.FromRatio != 0)
                sql.AddParameter("FromRatio", splitBonusEntity.FromRatio);
            if (splitBonusEntity.ToRatio != 0)
                sql.AddParameter("ToRatio", splitBonusEntity.ToRatio);
            sql.AddParameter("NseCode", splitBonusEntity.NseCode);
            sql.AddParameter("IsSplit", splitBonusEntity.IsSplit);
            sql.AddParameter("IsBonus", splitBonusEntity.IsBonus);
            sql.AddParameter("AnnounceDate", DbType.DateTime, ParameterDirection.Input, splitBonusEntity.AnnounceDate);
            sql.AddParameter("RewardDate", DbType.DateTime, ParameterDirection.Input, splitBonusEntity.RewardDate);
            sql.AddParameter("IsApply", splitBonusEntity.IsApply);
            return MyConvert.ToInt(await sql.ExecuteScalarAsync("SplitBonus_Insert", CommandType.StoredProcedure));
        }

        /// <summary>
        /// This function update record in SplitBonus table.
        /// </summary>
        /// <returns>PrimaryKey Field Value / AlreadyExist = 0</returns>
        public async Task<int> Update(SplitBonusEntity splitBonusEntity)
        {
            sql.AddParameter("Id", splitBonusEntity.Id);
            sql.AddParameter("NseCode", splitBonusEntity.NseCode);
            sql.AddParameter("IsSplit", splitBonusEntity.IsSplit);
            sql.AddParameter("IsBonus", splitBonusEntity.IsBonus);
            //if you insert zero in ui it will shows null in database
            if (splitBonusEntity.OldFaceValue != 0)
                sql.AddParameter("OldFaceValue", splitBonusEntity.OldFaceValue);
            if (splitBonusEntity.NewFaceValue != 0)
                sql.AddParameter("NewFaceValue", splitBonusEntity.NewFaceValue);
            if (splitBonusEntity.FromRatio != 0)
                sql.AddParameter("FromRatio", splitBonusEntity.FromRatio);
            if (splitBonusEntity.ToRatio != 0)
                sql.AddParameter("ToRatio", splitBonusEntity.ToRatio);
            sql.AddParameter("AnnounceDate", DbType.DateTime, ParameterDirection.Input, splitBonusEntity.AnnounceDate);
            sql.AddParameter("RewardDate", DbType.DateTime, ParameterDirection.Input, splitBonusEntity.RewardDate);

            return MyConvert.ToInt(await sql.ExecuteScalarAsync("SplitBonus_Update", CommandType.StoredProcedure));
        }

        /// <summary>
        /// This function delete record from SplitBonus table.
        /// </summary>
        public async Task Delete(int id)
        {
            sql.AddParameter("Id", id);
            await sql.ExecuteNonQueryAsync("SplitBonus_Delete", CommandType.StoredProcedure);
        }
        #endregion
        //public async Task SplitBonusApply(int id)
        //{
        //    sql.AddParameter("Id", id);
        //    await sql.ExecuteNonQueryAsync("SplitBonus_Apply", CommandType.StoredProcedure);
        //}
        public async Task<int> SplitBonusApply(int id,bool IsApply)
        {
            sql.AddParameter("Id", id);
            sql.AddParameter("IsApply", IsApply);
            return MyConvert.ToInt(await sql.ExecuteScalarAsync("SplitBonus_Apply", CommandType.StoredProcedure));
        }
    }
}
