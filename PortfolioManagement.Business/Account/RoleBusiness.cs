using CommonLibrary;
using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Account;
using PortfolioManagement.Repository.Account;
using System.Data;

namespace PortfolioManagement.Business.Account
{
    /// <summary>
    /// This class having crud operation function of table Role
    /// Created By :: Rekansh Patel
    /// Created On :: 02/03/2018
    /// </summary>
    public class RoleBusiness : CommonBusiness, IRoleRepository
    {
        ISql sql;

        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public RoleBusiness(IConfiguration configuration) : base(configuration)
        {
            sql = CreateSqlInstance();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This function return map reader table field to Entity of Role.
        /// </summary>
        /// <returns>Entity</returns>
        public RoleEntity MapData(IDataReader reader)
        {
            RoleEntity roleEntity = new RoleEntity();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "Id":
                        roleEntity.Id = MyConvert.ToInt(reader["Id"]);
                        break;
                    case "Name":
                        roleEntity.Name = MyConvert.ToString(reader["Name"]);
                        break;
                    case "IsPublic":
                        roleEntity.IsPublic = MyConvert.ToBoolean(reader["IsPublic"]);
                        break;
                }
            }
            return roleEntity;
        }

        /// <summary>
        /// This function return all columns values for perticular Role record
        /// </summary>
        /// <param name="Id">Perticular Record</param>
        /// <returns>Entity</returns>
        public async Task<RoleEntity> SelectForRecord(int Id)
        {
            sql.AddParameter("Id", Id);
            return await sql.ExecuteRecordAsync<RoleEntity>("Role_SelectForRecord", CommandType.StoredProcedure);
        }

        /// <summary>
        /// This function returns main informations for bind Role LOV
        /// </summary>
        /// <param name="roleParameterEntity">Filter criteria by Entity</param>
        /// <returns>Main Entitys</returns>
		public async Task<List<RoleMainEntity>> SelectForLOV(RoleParameterEntity roleParameterEntity)
        {
            if (roleParameterEntity.IsPublic != null)
                sql.AddParameter("IsPublic", roleParameterEntity.IsPublic);
            return await sql.ExecuteListAsync<RoleMainEntity>("Role_SelectForLOV", CommandType.StoredProcedure);
        }

        /// <summary>
        /// This function returns Role list page grid data.
        /// </summary>
        /// <param name="roleParameterEntity">Filter paramters</param>
        /// <returns>Role grid data</returns>
        public async Task<RoleGridEntity> SelectForGrid(RoleParameterEntity roleParameterEntity)
        {
            RoleGridEntity roleGridEntity = new RoleGridEntity();
            if (roleParameterEntity.Name != string.Empty)
                sql.AddParameter("Name", roleParameterEntity.Name);

            sql.AddParameter("SortExpression", roleParameterEntity.SortExpression);
            sql.AddParameter("SortDirection", roleParameterEntity.SortDirection);
            sql.AddParameter("PageIndex", roleParameterEntity.PageIndex);
            sql.AddParameter("PageSize", roleParameterEntity.PageSize);
            await sql.ExecuteEnumerableMultipleAsync<RoleGridEntity>("Role_SelectForGrid", CommandType.StoredProcedure, 2, roleGridEntity, MapGridEntity);
            return roleGridEntity;
        }

        /// <summary>
        /// This function map data for Role grid data
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="RoleGridEntity">Role grid data</param>
        /// <param name="reader">Database reader</param>
        public async Task MapGridEntity(int resultSet, RoleGridEntity roleGridEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    roleGridEntity.Roles.Add(await sql.MapDataDynamicallyAsync<RoleEntity>(reader));
                    break;
                case 1:
                    roleGridEntity.TotalRecords = MyConvert.ToInt(reader["TotalRecords"]);
                    break;
            }
        }

        /// <summary>
        /// This function insert record in Role table.
        /// </summary>
        /// <returns>Identity / AlreadyExist = 0</returns>
        public async Task<int> Insert(RoleEntity roleEntity)
        {
            sql.AddParameter("Name", roleEntity.Name);
            sql.AddParameter("IsPublic", roleEntity.IsPublic);
            return MyConvert.ToInt(await sql.ExecuteScalarAsync("Role_Insert", CommandType.StoredProcedure));
        }

        /// <summary>
        /// This function update record in Role table.
        /// </summary>
        /// <returns>PrimaryKey Field Value / AlreadyExist = 0</returns>
        public async Task<int> Update(RoleEntity roleEntity)
        {
            sql.AddParameter("Id", roleEntity.Id);
            sql.AddParameter("Name", roleEntity.Name);
            sql.AddParameter("IsPublic", roleEntity.IsPublic);
            return MyConvert.ToInt(await sql.ExecuteScalarAsync("Role_Update", CommandType.StoredProcedure));
        }

        /// <summary>
        /// This function delete record from Role table.
        /// </summary>
        public async Task Delete(int Id)
        {
            sql.AddParameter("Id", Id);
            await sql.ExecuteNonQueryAsync("Role_Delete", CommandType.StoredProcedure);
        }

        /// <summary>
        /// This function returns main informations for bind Role LOV
        /// </summary>
        /// <param name="roleParameterEntity">Filter criteria by Entity</param>
        /// <returns>Main Entitys</returns>
		public List<RoleEntity> SelectList()
        {
            return sql.ExecuteList<RoleEntity>("Role_Select", CommandType.StoredProcedure);
        }
        #endregion
    }

}
