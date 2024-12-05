using PortfolioManagement.Entity.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.Account
{
    public  interface IUserReppository
    {
        public Task<UserEntity> SelectForRecord(long Id);
        public  Task<List<UserMainEntity>> SelectForLOV(UserParameterEntity userParameterEntity);
        public  Task<UserAddEntity> SelectForAdd(UserParameterEntity userParameterEntity);
        public  Task MapAddEntity(int resultSet, UserAddEntity userAddEntity, IDataReader reader);
        public  Task<UserEditEntity> SelectForEdit(UserParameterEntity userParameterEntity);
        public  Task MapEditEntity(int resultSet, UserEditEntity userEditEntity, IDataReader reader);
        public  Task<UserGridEntity> SelectForGrid(UserParameterEntity userParameterEntity);
        public  Task MapGridEntity(int resultSet, UserGridEntity userGridEntity, IDataReader reader);
        public  Task<UserListEntity> SelectForList(UserParameterEntity userParameterEntity);
        public  Task MapListEntity(int resultSet, UserListEntity userListEntity, IDataReader reader);
        public  Task<long> Insert(UserEntity userEntity);
        public  Task<long> Registration(UserEntity userEntity);
        public  Task<List<UsersEntity>> SelectForUsersTest();
        public  Task<long> Update(UserEntity userEntity);
        public  Task Delete(long Id);
        public  Task<int> UserUpdate(UserUpdateEntity userUpdateEntity);
        public  Task<UserLoginEntity> ValidateLogin(UserEntity userEntity);
        public  Task<UserEntity> ResetPassword(string UsernameEmail);
        public  Task<bool> UpdateActive(UserEntity userEntity);
        public  Task<bool> UpdatePassword(UserEntity userEntity);
        public  Task<bool> ChangePassword(UserEntity userEntity);
        public  Task<bool> UpdatePasswordDirect(UserEntity userEntity);

    }
}
