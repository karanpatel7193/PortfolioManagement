using PortfolioManagement.Entity.Account;
using System.Data;

namespace PortfolioManagement.Repository.Account
{
    public interface IMenuRepository
    {
        public MenuEntity MapData(IDataReader reader);
        public Task<MenuEntity> SelectForRecord(int Id);
        public Task<MenuAddEntity> SelectForAdd(MenuParameterEntity menuParameterEntity);
        public Task MapAddEntity(int resultSet, MenuAddEntity menuAddEntity, IDataReader reader);
        public Task<MenuEditEntity> SelectForEdit(MenuParameterEntity menuParameterEntity);
        public Task MapEditEntity(int resultSet, MenuEditEntity menuEditEntity, IDataReader reader);
        public  Task<MenuGridEntity> SelectForGrid(MenuParameterEntity menuParameterEntity);
        public  Task<int> Insert(MenuEntity menuEntity);
        public  Task<int> Update(MenuEntity menuEntity);
        public  Task Delete(int Id);
        public  Task<List<MenuEntity>> SelectParent();
        public  Task<List<MenuEntity>> SelectChild(int ParentId);
        public List<MenuEntity> SelectList(MenuEntity menuEntity);

    }
}
