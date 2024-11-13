using CommonLibrary;
using CommonLibrary.SqlDB;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Account;
using PortfolioManagement.Entity.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.Master
{
    /// <summary>
    /// This class having crud operation function of table User
    /// Created By :: Rekansh Patel
    /// Created On :: 02/06/2018
    /// </summary>
    public class UserBusiness : CommonBusiness
    {
        ISql sql;

        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public UserBusiness(IConfiguration configuration) : base(configuration)
        {
            sql = CreateSqlInstance();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This function return map reader table field to Entity of User.
        /// </summary>
        /// <returns>Entity</returns>
        public UserEntity MapData(IDataReader reader)   
        {
            UserEntity userEntity = new UserEntity();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "Id":
                        userEntity.Id = MyConvert.ToLong(reader["Id"]);
                        break;
                    case "FirstName":
                        userEntity.FirstName = MyConvert.ToString(reader["FirstName"]);
                        break;
                    case "MiddleName":
                        userEntity.MiddleName = MyConvert.ToString(reader["MiddleName"]);
                        break;
                    case "LastName":
                        userEntity.LastName = MyConvert.ToString(reader["LastName"]);
                        break;
                    case "RoleId":
                        userEntity.RoleId = MyConvert.ToInt(reader["RoleId"]);
                        break;
                    case "Username":
                        userEntity.Username = MyConvert.ToString(reader["Username"]);
                        break;
                    case "Gender":
                        userEntity.Gender = MyConvert.ToInt(reader["Gender"]);
                        break;
                    case "Password":
                        userEntity.Password = MyConvert.ToString(reader["Password"]).ToDecrypt(MyConvert.ToString(reader["PasswordSalt"]).ToDecrypt());
                        break;
                    //case "PasswordSalt":
                    //    userEntity.PasswordSalt = MyConvert.ToString(reader["PasswordSalt"]);
                    //    break;
                    case "Email":
                        userEntity.Email = MyConvert.ToString(reader["Email"]);
                        break;
                    case "PhoneNumber":
                        userEntity.PhoneNumber = MyConvert.ToString(reader["PhoneNumber"]);
                        break;
                    case "ImageSrc":
                        userEntity.ImageSrc = MyConvert.ToString(reader["ImageSrc"]);
                        break;
                    case "IsActive":
                        userEntity.IsActive = MyConvert.ToBoolean(reader["IsActive"]);
                        break;
                    case "LastUpdateDateTime":
                        userEntity.LastUpdateDateTime = MyConvert.ToDateTime(reader["LastUpdateDateTime"]);
                        break;
                    case "PmsId":
                        userEntity.PmsId = MyConvert.ToInt(reader["PmsId"]);
                        break;
                    case "Type":
                        userEntity.Type = MyConvert.ToString(reader["Type"]);
                        break;

                }
            }
            return userEntity;
        }

        /// <summary>
        /// This function return all columns values for perticular User record
        /// </summary>
        /// <param name="Id">Perticular Record</param>
        /// <returns>Entity</returns>
        public async Task<UserEntity> SelectForRecord(long Id)
        {
            sql.AddParameter("Id", Id);
            return await sql.ExecuteRecordAsync<UserEntity>("User_SelectForRecord", CommandType.StoredProcedure);
        }

        /// <summary>
        /// This function returns main informations for bind User LOV
        /// </summary>
        /// <param name="userParameterEntity">Filter criteria by Entity</param>
        /// <returns>Main Entitys</returns>
		public async Task<List<UserMainEntity>> SelectForLOV(UserParameterEntity userParameterEntity)
        {
            return await sql.ExecuteListAsync<UserMainEntity>("User_SelectForLOV", CommandType.StoredProcedure);
        }

        /// <summary>
        /// This function return all LOVs data for User page add mode
        /// </summary>
        /// <param name="userParameterEntity">Parameter</param>
        /// <returns>Add modes all LOVs data</returns>
        public async Task<UserAddEntity> SelectForAdd(UserParameterEntity userParameterEntity)
        {
            UserAddEntity userAddEntity = new UserAddEntity();
            await sql.ExecuteEnumerableMultipleAsync<UserAddEntity>("User_SelectForAdd", CommandType.StoredProcedure, 1, userAddEntity, MapAddEntity);
            return userAddEntity;
        }

        /// <summary>
        /// This function map data for User page add mode LOVs
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="userAddEntity">Add mode Entity for fill data</param>
        /// <param name="reader">Database reader</param>
        public async Task MapAddEntity(int resultSet, UserAddEntity userAddEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    userAddEntity.Roles.Add(await sql.MapDataDynamicallyAsync<RoleMainEntity>(reader));
                    break;
            }
        }

        /// <summary>
        /// This function return all LOVs data and edit record information for User page edit mode
        /// </summary>
        /// <param name="userParameterEntity">Parameter</param>
        /// <returns>Edit modes all LOVs data and edit record information</returns>
        public async Task<UserEditEntity> SelectForEdit(UserParameterEntity userParameterEntity)
        {
            UserEditEntity userEditEntity = new UserEditEntity();
            if (userParameterEntity.Id != 0)
                sql.AddParameter("Id", userParameterEntity.Id);

            await sql.ExecuteEnumerableMultipleAsync<UserEditEntity>("User_SelectForEdit", CommandType.StoredProcedure, 2, userEditEntity, MapEditEntity);
            return userEditEntity;
        }

        /// <summary>
        /// This function map data for User page edit mode LOVs and edit record information
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="UserEditEntity">Edit mode Entity for fill data and edit record information</param>
        /// <param name="reader">Database reader</param>
        public async Task MapEditEntity(int resultSet, UserEditEntity userEditEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    userEditEntity.User = MapData(reader);
                    break;
                case 1:
                    userEditEntity.Roles.Add(await sql.MapDataDynamicallyAsync<RoleMainEntity>(reader));
                    break;
            }
        }

        /// <summary>
        /// This function returns User list page grid data.
        /// </summary>
        /// <param name="userParameterEntity">Filter paramters</param>
        /// <returns>User grid data</returns>
        public async Task<UserGridEntity> SelectForGrid(UserParameterEntity userParameterEntity)
        {
            UserGridEntity userGridEntity = new UserGridEntity();
            if (userParameterEntity.FirstName != string.Empty)
                sql.AddParameter("FirstName", userParameterEntity.FirstName);
            if (userParameterEntity.LastName != string.Empty)
                sql.AddParameter("LastName", userParameterEntity.LastName);
            if (userParameterEntity.Email != string.Empty)
                sql.AddParameter("Email", userParameterEntity.Email);
            if (userParameterEntity.RoleId != 0)
                sql.AddParameter("RoleId", userParameterEntity.RoleId);
            if (userParameterEntity.Username != string.Empty)
                sql.AddParameter("Username", userParameterEntity.Username);
            if (userParameterEntity.PhoneNumber != string.Empty)
                sql.AddParameter("PhoneNumber", userParameterEntity.PhoneNumber);

            sql.AddParameter("PmsId", userParameterEntity.PmsId);
            sql.AddParameter("SortExpression", userParameterEntity.SortExpression);
            sql.AddParameter("SortDirection", userParameterEntity.SortDirection);
            sql.AddParameter("PageIndex", userParameterEntity.PageIndex);
            sql.AddParameter("PageSize", userParameterEntity.PageSize);
            await sql.ExecuteEnumerableMultipleAsync<UserGridEntity>("User_SelectForGrid", CommandType.StoredProcedure, 2, userGridEntity, MapGridEntity);
            return userGridEntity;
        }

        /// <summary>
        /// This function map data for User grid data
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="UserGridEntity">User grid data</param>
        /// <param name="reader">Database reader</param>
        public async Task MapGridEntity(int resultSet, UserGridEntity userGridEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    userGridEntity.Users.Add(await sql.MapDataDynamicallyAsync<UserEntity>(reader));
                    break;
                case 1:
                    userGridEntity.TotalRecords = MyConvert.ToInt(reader["TotalRecords"]);
                    break;
            }
        }

        /// <summary>
        /// This function returns User list page grid data and LOV data
        /// </summary>
        /// <param name="userParameterEntity">Filter paramters</param>
        /// <returns>User grid data and LOV data</returns>
        public async Task<UserListEntity> SelectForList(UserParameterEntity userParameterEntity)
        {
            UserListEntity userListEntity = new UserListEntity();
            if (userParameterEntity.FirstName != string.Empty)
                sql.AddParameter("FirstName", userParameterEntity.FirstName);
            if (userParameterEntity.LastName != string.Empty)
                sql.AddParameter("LastName", userParameterEntity.LastName);
            if (userParameterEntity.Email != string.Empty)
                sql.AddParameter("Email", userParameterEntity.Email);
            if (userParameterEntity.RoleId != 0)
                sql.AddParameter("RoleId", userParameterEntity.RoleId);
            if (userParameterEntity.Username != string.Empty)
                sql.AddParameter("Username", userParameterEntity.Username);
            if (userParameterEntity.PhoneNumber != string.Empty)
                sql.AddParameter("PhoneNumber", userParameterEntity.PhoneNumber);

            sql.AddParameter("PmsId", userParameterEntity.PmsId);
            sql.AddParameter("SortExpression", userParameterEntity.SortExpression);
            sql.AddParameter("SortDirection", userParameterEntity.SortDirection);
            sql.AddParameter("PageIndex", userParameterEntity.PageIndex);
            sql.AddParameter("PageSize", userParameterEntity.PageSize);
            await sql.ExecuteEnumerableMultipleAsync<UserListEntity>("User_SelectForList", CommandType.StoredProcedure, 3, userListEntity, MapListEntity);
            return userListEntity;
        }

        /// <summary>
        /// This function map data for User list page grid data and LOV data
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="UserListEntity">User list page grid data and LOV data</param>
        /// <param name="reader">Database reader</param>
        public async Task MapListEntity(int resultSet, UserListEntity userListEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    userListEntity.Roles.Add(await sql.MapDataDynamicallyAsync<RoleMainEntity>(reader));
                    break;
                case 1:
                    userListEntity.Users.Add(await sql.MapDataDynamicallyAsync<UserEntity>(reader));
                    break;
                case 2:
                    userListEntity.TotalRecords = MyConvert.ToInt(reader["TotalRecords"]);
                    break;
            }
        }

        /// <summary>
        /// This function insert record in User table.
        /// </summary>
        /// <returns>Identity / AlreadyExist = 0</returns>
        public async Task<long> Insert(UserEntity userEntity)
        {
            string sPasswordSalt = CommonMethods.GetRandomString();

            sql.AddParameter("FirstName", userEntity.FirstName);
            sql.AddParameter("LastName", userEntity.LastName);
            sql.AddParameter("Username", userEntity.Username);
            sql.AddParameter("Password", userEntity.Password.ToEncrypt(sPasswordSalt));
            sql.AddParameter("PasswordSalt", sPasswordSalt.ToEncrypt());
            sql.AddParameter("LastUpdateDateTime", DbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            sql.AddParameter("RoleId", userEntity.RoleId);
            sql.AddParameter("Gender", userEntity.Gender);
            sql.AddParameter("Email", userEntity.Email);
            sql.AddParameter("IsActive", userEntity.IsActive);
            sql.AddParameter("PhoneNumber", userEntity.PhoneNumber);
            if (userEntity.ImageSrc != string.Empty)
                sql.AddParameter("ImageSrc", userEntity.ImageSrc);          
            if (userEntity.MiddleName != string.Empty)
                sql.AddParameter("MiddleName", userEntity.MiddleName);
            if (userEntity.BirthDate != DateTime.MinValue)
                sql.AddParameter("BirthDate", DbType.DateTime, ParameterDirection.Input, userEntity.BirthDate);
            sql.AddParameter("PmsId", userEntity.PmsId);
            return MyConvert.ToLong(await sql.ExecuteScalarAsync("User_Insert", CommandType.StoredProcedure));
        }

        public async Task<long> Registration(UserEntity userEntity)
        {
            string sPasswordSalt = CommonMethods.GetRandomString();

            sql.AddParameter("FirstName", userEntity.FirstName);
            sql.AddParameter("LastName", userEntity.LastName);
            sql.AddParameter("Username", userEntity.Username);
            sql.AddParameter("Password", userEntity.Password.ToEncrypt(sPasswordSalt));
            sql.AddParameter("PasswordSalt", sPasswordSalt.ToEncrypt());
            sql.AddParameter("LastUpdateDateTime", DbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            sql.AddParameter("Email", userEntity.Email);
            if (userEntity.PhoneNumber != string.Empty)
                sql.AddParameter("PhoneNumber", userEntity.PhoneNumber);
            sql.AddParameter("Type", userEntity.Type);
            sql.AddParameter("RoleId", userEntity.RoleId);
            if (userEntity.ImageSrc != string.Empty)
                sql.AddParameter("ImageSrc", userEntity.ImageSrc);
            if (userEntity.MiddleName != string.Empty)
                sql.AddParameter("MiddleName", userEntity.MiddleName);
            if (userEntity.BirthDate != DateTime.MinValue)
                sql.AddParameter("BirthDate", DbType.DateTime, ParameterDirection.Input, userEntity.BirthDate);
            if (userEntity.IsActive != false)
                sql.AddParameter("IsActive", userEntity.IsActive);
            if (userEntity.Gender != 0)
                sql.AddParameter("Gender", userEntity.Gender);

            if (userEntity.Type == "Individual")
            {
                userEntity.PmsName = userEntity.FirstName + " " + userEntity.LastName;
                sql.AddParameter("PmsName", userEntity.PmsName);
            }
            else
            {
                sql.AddParameter("PmsName", userEntity.PmsName);
            }
            return MyConvert.ToLong(await sql.ExecuteScalarAsync("User_Registration", CommandType.StoredProcedure));
        }

        public async Task<List<UsersEntity>> SelectForUsersTest()
        {
            return await sql.ExecuteListAsync<UsersEntity>("users_select", CommandType.StoredProcedure);
        }

        /// <summary>
        /// This function update record in User table.
        /// </summary>
        /// <returns>PrimaryKey Field Value / AlreadyExist = 0</returns>
        public async Task<long> Update(UserEntity userEntity)
        {
            string sPasswordSalt = CommonMethods.GetRandomString();
            sql.AddParameter("Id", userEntity.Id);
            sql.AddParameter("FirstName", userEntity.FirstName);
            if (userEntity.MiddleName != string.Empty)
                sql.AddParameter("MiddleName", userEntity.MiddleName);
            sql.AddParameter("LastName", userEntity.LastName);
            sql.AddParameter("RoleId", userEntity.RoleId);
            sql.AddParameter("Username", userEntity.Username);
            sql.AddParameter("Password", userEntity.Password.ToEncrypt(sPasswordSalt));
            sql.AddParameter("PasswordSalt", sPasswordSalt.ToEncrypt());
            if (userEntity.Gender != 0)
                sql.AddParameter("Gender", userEntity.Gender);
            if (userEntity.BirthDate != DateTime.MinValue)
                sql.AddParameter("BirthDate", DbType.DateTime, ParameterDirection.Input, userEntity.BirthDate);
            sql.AddParameter("Email", userEntity.Email);
            if (userEntity.PhoneNumber != string.Empty)
                sql.AddParameter("PhoneNumber", userEntity.PhoneNumber);
            sql.AddParameter("IsActive", userEntity.IsActive);
            sql.AddParameter("LastUpdateDateTime", DbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            return MyConvert.ToLong(await sql.ExecuteScalarAsync("User_Update", CommandType.StoredProcedure));
        }

        /// <summary>
        /// This function delete record from User table.
        /// </summary>
        public async Task Delete(long Id)
        {
            sql.AddParameter("Id", Id);
            await sql.ExecuteNonQueryAsync("User_Delete", CommandType.StoredProcedure);
        }
        #endregion

        #region other public methods
        /// <summary>
        /// This function return UserEntity List from User based on pass parameter.
        /// </summary>
        /// <returns>List</returns>
        public async Task<UserLoginEntity> ValidateLogin(UserEntity userEntity)
        {
            if (userEntity.Username != string.Empty)
                sql.AddParameter("Username", userEntity.Username);
            string sPasswordSalt = MyConvert.ToString(await sql.ExecuteScalarAsync("User_SelectPasswordSalt", CommandType.StoredProcedure)).ToDecrypt();

            sql.AddParameter("Username", userEntity.Username);
            sql.AddParameter("Password", userEntity.Password.ToEncrypt(sPasswordSalt));
            sql.AddParameter("Now", DbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);

            return await sql.ExecuteRecordAsync<UserLoginEntity>("User_SelectValidLogin", CommandType.StoredProcedure);
        }

        public async Task<UserEntity> ResetPassword(string UsernameEmail)
        {
            sql.AddParameter("UsernameEmail", UsernameEmail);
            return await sql.ExecuteRecordAsync<UserEntity>("User_SelectForResetPassword", CommandType.StoredProcedure);
        }

        public async Task<bool> UpdateActive(UserEntity userEntity)
        {
            sql.AddParameter("Id", userEntity.Id);
            sql.AddParameter("Email", userEntity.Email);
            sql.AddParameter("IsActive", userEntity.IsActive);
            sql.AddParameter("LastUpdateDateTime", DateTime.UtcNow);
            return MyConvert.ToBoolean(await sql.ExecuteScalarAsync("User_UpdateActive", CommandType.StoredProcedure));
        }

        public async Task<bool> UpdatePassword(UserEntity userEntity)
        {
            string sPasswordSalt = CommonMethods.GetRandomString();
            sql.AddParameter("Id", userEntity.Id);
            sql.AddParameter("Password", userEntity.Password.ToEncrypt(sPasswordSalt));
            sql.AddParameter("PasswordSalt", sPasswordSalt.ToEncrypt());
            sql.AddParameter("LastUpdateDateTime", userEntity.LastUpdateDateTime);
            sql.AddParameter("Email", userEntity.Email);
            return MyConvert.ToBoolean(await sql.ExecuteScalarAsync("User_UpdatePassword", CommandType.StoredProcedure));
        }

        public async Task<bool> ChangePassword(UserEntity userEntity)
        {
            if (userEntity.Id > 0)
                sql.AddParameter("Id", userEntity.Id);
            UserEntity userPasswordEntity = await sql.ExecuteRecordAsync<UserEntity>("User_SelectForChangePassword", CommandType.StoredProcedure);
            string sPasswordSalt = MyConvert.ToString(userPasswordEntity.PasswordSalt).ToDecrypt();
            if (userEntity.OldPassword == userPasswordEntity.Password.ToDecrypt(sPasswordSalt))
            {
                sql.AddParameter("Id", userEntity.Id);
                sql.AddParameter("Password", userEntity.Password.ToEncrypt(sPasswordSalt));
                sql.AddParameter("PasswordSalt", sPasswordSalt.ToEncrypt());
                sql.AddParameter("LastUpdateDateTime", userEntity.LastUpdateDateTime);
                return MyConvert.ToBoolean(await sql.ExecuteScalarAsync("User_UpdatePasswordDirect", CommandType.StoredProcedure));
            }
            else
                return false;
        }

        public async Task<bool> UpdatePasswordDirect(UserEntity userEntity)
        {
            string sPasswordSalt = CommonMethods.GetRandomString();
            sql.AddParameter("Id", userEntity.Id);
            sql.AddParameter("Password", userEntity.Password.ToEncrypt(sPasswordSalt));
            sql.AddParameter("PasswordSalt", sPasswordSalt.ToEncrypt());
            sql.AddParameter("LastUpdateDateTime", userEntity.LastUpdateDateTime);
            return MyConvert.ToBoolean(await sql.ExecuteScalarAsync("User_UpdatePasswordDirect", CommandType.StoredProcedure));
        }
        #endregion

    }
}
