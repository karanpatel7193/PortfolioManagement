using CommonLibrary;
using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Account;
using PortfolioManagement.Repository.Account;
using System.Data;

namespace PortfolioManagement.Business.Account
{
    /// <summary>
    /// This class having crud operation function of table Menu
    /// Created By :: Rekansh Patel
    /// Created On :: 02/03/2018
    /// </summary>
    public class MenuBusiness : CommonBusiness, IMenuRepository
    {
        ISql sql;

        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        
        public MenuBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This function return map reader table field to Entity of Menu.
        /// </summary>
        /// <returns>Entity</returns>
        public MenuEntity MapData(IDataReader reader)
        {
            MenuEntity menuEntity = new MenuEntity();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "Id":
                        menuEntity.Id = MyConvert.ToInt(reader["Id"]);
                        break;
                    case "Name":
                        menuEntity.Name = MyConvert.ToString(reader["Name"]);
                        break;
                    case "Description":
                        menuEntity.Description = MyConvert.ToString(reader["Description"]);
                        break;
                    case "ParentId":
                        menuEntity.ParentId = MyConvert.ToInt(reader["ParentId"]);
                        break;
                    case "PageTitle":
                        menuEntity.PageTitle = MyConvert.ToString(reader["PageTitle"]);
                        break;
                    case "Icon":
                        menuEntity.Icon = MyConvert.ToString(reader["Icon"]);
                        break;
                    case "Routing":
                        menuEntity.Routing = MyConvert.ToString(reader["Routing"]);
                        break;
                    case "OrderBy":
                        menuEntity.OrderBy = MyConvert.ToInt(reader["OrderBy"]);
                        break;
                    case "IsMenu":
                        menuEntity.IsMenu = MyConvert.ToBoolean(reader["IsMenu"]);
                        break;
                    case "IsClient":
                        menuEntity.IsClient = MyConvert.ToBoolean(reader["IsClient"]);
                        break;
                    case "IsPublic":
                        menuEntity.IsPublic = MyConvert.ToBoolean(reader["IsPublic"]);
                        break;
                }
            }
            return menuEntity;
        }

        /// <summary>
        /// This function return all columns values for perticular Menu record
        /// </summary>
        /// <param name="Id">Perticular Record</param>
        /// <returns>Entity</returns>
        public async Task<MenuEntity> SelectForRecord(int Id)
        {
            sql.AddParameter("Id", Id);
            return await sql.ExecuteRecordAsync<MenuEntity>("Menu_SelectForRecord", CommandType.StoredProcedure);
        }

        /// <summary>
        /// This function return all LOVs data for Menu page add mode
        /// </summary>
        /// <param name="menuParameterEntity">Parameter</param>
        /// <returns>Add modes all LOVs data</returns>
        public async Task<MenuAddEntity> SelectForAdd(MenuParameterEntity menuParameterEntity)
        {
            MenuAddEntity menuAddEntity = new MenuAddEntity();

            await sql.ExecuteEnumerableMultipleAsync<MenuAddEntity>("Menu_SelectForAdd", CommandType.StoredProcedure, 1, menuAddEntity, MapAddEntity);
            return menuAddEntity;
        }

        /// <summary>
        /// This function map data for Menu page add mode LOVs
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="menuAddEntity">Add mode Entity for fill data</param>
        /// <param name="reader">Database reader</param>
        public async Task MapAddEntity(int resultSet, MenuAddEntity menuAddEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    menuAddEntity.Menus.Add(await sql.MapDataDynamicallyAsync<MenuMainEntity>(reader));
                    break;
            }
        }

        /// <summary>
        /// This function return all LOVs data and edit record information for Menu page edit mode
        /// </summary>
        /// <param name="menuParameterEntity">Parameter</param>
        /// <returns>Edit modes all LOVs data and edit record information</returns>
        public async Task<MenuEditEntity> SelectForEdit(MenuParameterEntity menuParameterEntity)
        {
            MenuEditEntity menuEditEntity = new MenuEditEntity();
            sql.AddParameter("Id", menuParameterEntity.Id);
            await sql.ExecuteEnumerableMultipleAsync<MenuEditEntity>("Menu_SelectForEdit", CommandType.StoredProcedure, 2, menuEditEntity, MapEditEntity);
            return menuEditEntity;
        }

        /// <summary>
        /// This function map data for Menu page edit mode LOVs and edit record information
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="MenuEditEntity">Edit mode Entity for fill data and edit record information</param>
        /// <param name="reader">Database reader</param>
        public async Task MapEditEntity(int resultSet, MenuEditEntity menuEditEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    menuEditEntity.Menu = await sql.MapDataDynamicallyAsync<MenuEntity>(reader);
                    break;
                case 1:
                    menuEditEntity.Menus.Add(await sql.MapDataDynamicallyAsync<MenuMainEntity>(reader));
                    break;

            }
        }

        /// <summary>
        /// This function returns Menu list page grid data.
        /// </summary>
        /// <param name="menuParameterEntity">Filter paramters</param>
        /// <returns>Menu grid data</returns>
        public async Task<MenuGridEntity> SelectForGrid(MenuParameterEntity menuParameterEntity)
        {
            MenuGridEntity menuGridEntity = new MenuGridEntity();
            if (menuParameterEntity.Name != string.Empty)
                sql.AddParameter("Name", menuParameterEntity.Name);
            if (menuParameterEntity.ParentId != 0)
                sql.AddParameter("ParentId", menuParameterEntity.ParentId);

            sql.AddParameter("SortExpression", menuParameterEntity.SortExpression);
            sql.AddParameter("SortDirection", menuParameterEntity.SortDirection);
            sql.AddParameter("PageIndex", menuParameterEntity.PageIndex);
            sql.AddParameter("PageSize", menuParameterEntity.PageSize);
            await sql.ExecuteEnumerableMultipleAsync<MenuGridEntity>("Menu_SelectForGrid", CommandType.StoredProcedure, 2, menuGridEntity, MapGridEntity);
            return menuGridEntity;
        }

        /// <summary>
        /// This function map data for Menu grid data
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="MenuGridEntity">Menu grid data</param>
        /// <param name="reader">Database reader</param>
        public async Task MapGridEntity(int resultSet, MenuGridEntity menuGridEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    menuGridEntity.Menus.Add(await sql.MapDataDynamicallyAsync<MenuEntity>(reader));
                    break;
                case 1:
                    menuGridEntity.TotalRecords = MyConvert.ToInt(reader["TotalRecords"]);
                    break;
            }
        }

        /// <summary>
        /// This function insert record in Menu table.
        /// </summary>
        /// <returns>Identity / AlreadyExist = 0</returns>
        public async Task<int> Insert(MenuEntity menuEntity)
        {
            sql.AddParameter("Name", menuEntity.Name);
            if (menuEntity.Description != string.Empty)
                sql.AddParameter("Description", menuEntity.Description);
            if (menuEntity.ParentId != 0)
                sql.AddParameter("ParentId", menuEntity.ParentId);
            if (menuEntity.PageTitle != string.Empty)
                sql.AddParameter("PageTitle", menuEntity.PageTitle);
            if (menuEntity.Icon != string.Empty)
                sql.AddParameter("Icon", menuEntity.Icon);
            if (menuEntity.Routing != string.Empty)
                sql.AddParameter("Routing", menuEntity.Routing);
            if (menuEntity.OrderBy != 0)
                sql.AddParameter("OrderBy", menuEntity.OrderBy);
            sql.AddParameter("IsMenu", menuEntity.IsMenu);
            sql.AddParameter("IsClient", menuEntity.IsClient);
            sql.AddParameter("IsPublic", menuEntity.IsPublic);
            return MyConvert.ToInt(await sql.ExecuteScalarAsync("Menu_Insert", CommandType.StoredProcedure));
        }

        /// <summary>
        /// This function update record in Menu table.
        /// </summary>
        /// <returns>PrimaryKey Field Value / AlreadyExist = 0</returns>
        public async Task<int> Update(MenuEntity menuEntity)
        {
            sql.AddParameter("Id", menuEntity.Id);
            sql.AddParameter("Name", menuEntity.Name);
            if (menuEntity.Description != string.Empty)
                sql.AddParameter("Description", menuEntity.Description);
            if (menuEntity.ParentId != 0)
                sql.AddParameter("ParentId", menuEntity.ParentId);
            if (menuEntity.PageTitle != string.Empty)
                sql.AddParameter("PageTitle", menuEntity.PageTitle);
            if (menuEntity.Icon != string.Empty)
                sql.AddParameter("Icon", menuEntity.Icon);
            if (menuEntity.Routing != string.Empty)
                sql.AddParameter("Routing", menuEntity.Routing);
            if (menuEntity.OrderBy != 0)
                sql.AddParameter("OrderBy", menuEntity.OrderBy);
            sql.AddParameter("IsMenu", menuEntity.IsMenu);
            sql.AddParameter("IsClient", menuEntity.IsClient);
            sql.AddParameter("IsPublic", menuEntity.IsPublic);
            return MyConvert.ToInt(await sql.ExecuteScalarAsync("Menu_Update", CommandType.StoredProcedure));
        }

        /// <summary>
        /// This function delete record from Menu table.
        /// </summary>
        public async Task Delete(int Id)
        {
            sql.AddParameter("Id", Id);
            await sql.ExecuteNonQueryAsync("Menu_Delete", CommandType.StoredProcedure);
        }
        #endregion

        #region Public Other Methods
        /// <summary>
        /// This function return all parent menus.
        /// </summary>
        /// <returns>List</returns>
        public async Task<List<MenuEntity>> SelectParent()
        {
            return await sql.ExecuteListAsync<MenuEntity>("Menu_SelectParent", CommandType.StoredProcedure);
        }

        /// <summary>
        /// This function return all child menus by parent id.
        /// </summary>
        /// <returns>List</returns>
        public async Task<List<MenuEntity>> SelectChild(int ParentId)
        {
            sql.AddParameter("ParentId", ParentId);
            return await sql.ExecuteListAsync<MenuEntity>("Menu_SelectChild", CommandType.StoredProcedure);
        }

        /// <summary>
        /// This function return MenuEntity List from Menu based on pass parameter.
        /// </summary>
        /// <returns>List</returns>
        public List<MenuEntity> SelectList(MenuEntity menuEntity)
        {
            if (menuEntity != null)
            {
                if (menuEntity.Id != 0)
                    sql.AddParameter("Id", menuEntity.Id);
                if (menuEntity.Name != string.Empty)
                    sql.AddParameter("Name", menuEntity.Name);
            }
            return sql.ExecuteList<MenuEntity>("Menu_Select", CommandType.StoredProcedure);
        }
        #endregion
    }
}
