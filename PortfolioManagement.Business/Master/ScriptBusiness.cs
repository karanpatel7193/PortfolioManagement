using CommonLibrary;
using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Business;
using PortfolioManagement.Entity.Master;
using PortfolioManagement.Repository.Master;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.Master
{
    /// <summary>
    /// This class having crud operation function of table Script
    /// Created By :: Rekansh Patel
    /// Created On :: 10/30/2020
    /// </summary>
    public class ScriptBusiness : CommonBusiness, IScriptRepository
    {
        ISql sql;

        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public ScriptBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This function return map reader table field to Entity of Script.
        /// </summary>
        /// <returns>Entity</returns>
        public ScriptEntity MapData(IDataReader reader)
        {
            ScriptEntity scriptEntity = new ScriptEntity();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "Id":
                        scriptEntity.Id = MyConvert.ToInt(reader["Id"]);
                        break;
                    case "Name":
                        scriptEntity.Name = MyConvert.ToString(reader["Name"]);
                        break;
                    case "BseCode":
                        scriptEntity.BseCode = MyConvert.ToDecimal(reader["BseCode"]);
                        break;
                    case "NseCode":
                        scriptEntity.NseCode = MyConvert.ToString(reader["NseCode"]);
                        break;
                    case "IciciCode":
                        scriptEntity.IciciCode = MyConvert.ToString(reader["IciciCode"]);
                        break;
                    case "ISINCode":
                        scriptEntity.ISINCode = MyConvert.ToString(reader["ISINCode"]);
                        break;
                    case "MoneyControlURL":
                        scriptEntity.MoneyControlURL = MyConvert.ToString(reader["MoneyControlURL"]);
                        break;
                    case "FetchURL":
                        scriptEntity.FetchURL = MyConvert.ToString(reader["FetchURL"]);
                        break;
                    case "IsFetch":
                        scriptEntity.IsFetch = MyConvert.ToBoolean(reader["IsFetch"]);
                        break;
                    case "IsActive":
                        scriptEntity.IsActive = MyConvert.ToBoolean(reader["IsActive"]);
                        break;
                    case "IndustryName":
                        scriptEntity.IndustryName = MyConvert.ToString(reader["IndustryName"]);
                        break;
                    case "Group":
                        scriptEntity.Group = MyConvert.ToString(reader["Group"]);
                        break;
                    case "FaceValue":
                        scriptEntity.FaceValue = MyConvert.ToInt(reader["FaceValue"]);
                        break;
                }
            }
            return scriptEntity;
        }

        /// <summary>
        /// This function return all columns values for particular Script record
        /// </summary>
        /// <param name="Id">Particular Record</param>
        /// <returns>Entity</returns>
        public async Task<ScriptEntity> SelectForRecord(int id)
        {
            sql.AddParameter("Id", id);
            return await sql.ExecuteRecordAsync<ScriptEntity>("Script_SelectForRecord", CommandType.StoredProcedure);
        }

        /// <summary>
        /// This function returns main informations for bind Script LOV
        /// </summary>
        /// <param name="scriptEntity">Filter criteria by Entity</param>
        /// <returns>Main Entitys</returns>
		public async Task<List<ScriptMainEntity>> SelectForLOV(ScriptParameterEntity scriptParameterEntity)
        {
            if (scriptParameterEntity.Id != 0)
                sql.AddParameter("Id", scriptParameterEntity.Id);
            if (scriptParameterEntity.Name != string.Empty)
                sql.AddParameter("Name", scriptParameterEntity.Name);
            return await sql.ExecuteListAsync<ScriptMainEntity>("Script_SelectForLOV", CommandType.StoredProcedure);
        }

        /// <summary>
        /// This function returns Script list page grid data.
        /// </summary>
        /// <param name="scriptParameterEntity">Filter paramters</param>
        /// <returns>Script grid data</returns>
        public async Task<ScriptGridEntity> SelectForGrid(ScriptParameterEntity scriptParameterEntity)
        {
            ScriptGridEntity scriptGridEntity = new ScriptGridEntity();
            if (scriptParameterEntity.Id != 0)
                sql.AddParameter("Id", scriptParameterEntity.Id);
            if (scriptParameterEntity.Name != string.Empty)
                sql.AddParameter("Name", scriptParameterEntity.Name);

            if (scriptParameterEntity.NseCode != string.Empty)
                sql.AddParameter("NseCode", scriptParameterEntity.NseCode);
            if (scriptParameterEntity.BseCode != 0)
                sql.AddParameter("BseCode", scriptParameterEntity.BseCode);

            sql.AddParameter("SortExpression", scriptParameterEntity.SortExpression);
            sql.AddParameter("SortDirection", scriptParameterEntity.SortDirection);
            sql.AddParameter("PageIndex", scriptParameterEntity.PageIndex);
            sql.AddParameter("PageSize", scriptParameterEntity.PageSize);
            await sql.ExecuteEnumerableMultipleAsync<ScriptGridEntity>("Script_SelectForGrid", CommandType.StoredProcedure, 2, scriptGridEntity, MapGridEntity);
            return scriptGridEntity;
        }

        /// <summary>
        /// This function map data for Script grid data
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="scriptGridEntity">Script grid data</param>
        /// <param name="reader">Database reader</param>
        public async Task MapGridEntity(int resultSet, ScriptGridEntity scriptGridEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    scriptGridEntity.Scripts.Add(await sql.MapDataDynamicallyAsync<ScriptEntity>(reader));
                    break;
                case 1:
                    scriptGridEntity.TotalRecords = MyConvert.ToInt(reader["TotalRecords"]);
                    break;
            }
        }

        /// <summary>
        /// This function insert record in Script table.
        /// </summary>
        /// <returns>Identity / AlreadyExist = 0</returns>
        public async Task<int> Insert(ScriptEntity scriptEntity)
        {   
            sql.AddParameter("Name", scriptEntity.Name);
            sql.AddParameter("BseCode", scriptEntity.BseCode);
            sql.AddParameter("NseCode", scriptEntity.NseCode);
            //sql.AddParameter("IciciCode", scriptEntity.IciciCode);
            sql.AddParameter("ISINCode", scriptEntity.ISINCode);
            sql.AddParameter("MoneyControlURL", scriptEntity.MoneyControlURL);
            sql.AddParameter("FetchURL", scriptEntity.FetchURL);
            sql.AddParameter("IsFetch", scriptEntity.IsFetch);
            sql.AddParameter("IsActive", scriptEntity.IsActive);
            sql.AddParameter("IndustryName", scriptEntity.IndustryName);
            sql.AddParameter("Group", scriptEntity.Group);
            sql.AddParameter("FaceValue", scriptEntity.FaceValue);
            return MyConvert.ToInt(await sql.ExecuteScalarAsync("Script_Insert", CommandType.StoredProcedure));
        }

        /// <summary>
        /// This function update record in Script table.
        /// </summary>
        /// <returns>PrimaryKey Field Value / AlreadyExist = 0</returns>
        public async Task<int> Update(ScriptEntity scriptEntity)
        {
            sql.AddParameter("Id", scriptEntity.Id);
            sql.AddParameter("Name", scriptEntity.Name);
            sql.AddParameter("BseCode", scriptEntity.BseCode);
            sql.AddParameter("NseCode", scriptEntity.NseCode);
            //sql.AddParameter("IciciCode", scriptEntity.IciciCode);
            sql.AddParameter("ISINCode", scriptEntity.ISINCode);
            sql.AddParameter("MoneyControlURL", scriptEntity.MoneyControlURL);
            sql.AddParameter("FetchURL", scriptEntity.FetchURL);
            sql.AddParameter("IsFetch", scriptEntity.IsFetch);
            sql.AddParameter("IsActive", scriptEntity.IsActive);
            sql.AddParameter("IndustryName", scriptEntity.IndustryName);
            sql.AddParameter("Group", scriptEntity.Group);
            sql.AddParameter("FaceValue", scriptEntity.FaceValue);
            return MyConvert.ToInt(await sql.ExecuteScalarAsync("Script_Update", CommandType.StoredProcedure));
        }

        /// <summary>
        /// This function delete record from Script table.
        /// </summary>
        public async Task Delete(int id)
        {
            sql.AddParameter("Id", id);
            await sql.ExecuteNonQueryAsync("Script_Delete", CommandType.StoredProcedure);
        }

        /// <summary>
        /// This function returns main informations for script scraping
        /// </summary>
        /// <returns>Entitys</returns>
		public async Task<List<ScriptEntity>> SelectForScrap()
        {
            return await sql.ExecuteListAsync<ScriptEntity>("Script_SelectForScrap", CommandType.StoredProcedure);
        }
        #endregion
    }

}
