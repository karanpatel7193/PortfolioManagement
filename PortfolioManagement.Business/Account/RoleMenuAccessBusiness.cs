using CommonLibrary;
using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Account;
using System.Data;

namespace PortfolioManagement.Business.Account
{
    /// <summary>   
    /// This class having crud operation function of table RoleMenuAccess   
    /// Created By :: Rekansh Patel   
    /// Created On :: 05/27/2017   
    /// </summary>   
    public class RoleMenuAccessBusiness : CommonBusiness
    {
        ISql sql;

        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public RoleMenuAccessBusiness(IConfiguration configuration) : base(configuration)
        {
            sql = CreateSqlInstance();
        }
        #endregion

        #region public override Methods
        /// <summary>      
        /// This function return map reader table field to Entity of RoleMenuAccess.      
        /// </summary>      
        /// <returns>Entity</returns>          
        public RoleMenuAccessEntity MapData(IDataReader reader)
        {
            RoleMenuAccessEntity roleMenuAccessEntity = new RoleMenuAccessEntity();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "Id":
                        roleMenuAccessEntity.Id = MyConvert.ToInt(reader["Id"]);
                        break;
                    case "RoleId":
                        roleMenuAccessEntity.RoleId = MyConvert.ToInt(reader["RoleId"]);
                        break;
                    case "MenuId":
                        roleMenuAccessEntity.MenuId = MyConvert.ToInt(reader["MenuId"]);
                        break;
                    case "CanInsert":
                        roleMenuAccessEntity.CanInsert = MyConvert.ToBoolean(reader["CanInsert"]);
                        break;
                    case "CanUpdate":
                        roleMenuAccessEntity.CanUpdate = MyConvert.ToBoolean(reader["CanUpdate"]);
                        break;
                    case "CanDelete":
                        roleMenuAccessEntity.CanDelete = MyConvert.ToBoolean(reader["CanDelete"]);
                        break;
                    case "CanView":
                        roleMenuAccessEntity.CanView = MyConvert.ToBoolean(reader["CanView"]);
                        break;
                    case "MenuIdName":
                        roleMenuAccessEntity.MenuIdName = MyConvert.ToString(reader["MenuIdName"]);
                        break;
                }
            }
            return roleMenuAccessEntity;

        }

        /// <summary>
        /// This function return MenuEntity List from Menu based on pass parameter.
        /// </summary>
        /// <returns>List</returns>
        public List<RoleMenuAccessEntity> SelectList()
        {
            return sql.ExecuteList<RoleMenuAccessEntity>("RoleMenuAccess_Select", CommandType.StoredProcedure);
        }

        /// <summary>      
        /// This function return RoleMenuAccessEntity List from RoleMenuAccess based on pass parameter.      
        /// </summary>      
        /// <returns>List</returns>   
        public async Task<List<RoleMenuAccessEntity>> SelectListByRoleIdParentId(RoleMenuAccessEntity roleMenuAccessEntity)
        {
            sql.AddParameter("RoleId", roleMenuAccessEntity.RoleId);
            sql.AddParameter("ParentId", roleMenuAccessEntity.MenuId);
            return await sql.ExecuteListAsync<RoleMenuAccessEntity>("RoleMenuAccess_SelectByRoleIdParentId", CommandType.StoredProcedure);
        }

        /// <summary>      
        /// This function bulk insert/update/delete record in RoleMenuAccess table.      
        /// </summary>      
        /// <returns>Identity / AlreadyExist = 0</returns>      
        public async Task<int> Bulk(RoleEntity roleEntity)
        {
            sql.AddParameter("RoleId", roleEntity.Id);
            sql.AddParameter("AccessXML", roleEntity.RoleMenuAccesss.ToXML());
            return MyConvert.ToInt(await sql.ExecuteScalarAsync("RoleMenuAccess_Bulk", CommandType.StoredProcedure));

        }
        #endregion
    }

}
