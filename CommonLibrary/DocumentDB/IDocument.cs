using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommonLibrary.DocumentDB
{
    public interface IDocument<TEntity>
    {
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }

        string CollectionName { get; set; }

        List<TEntity> GetAll();

        Task<List<TEntity>> GetAllAsync();

        TEntity GetById(string id);

        Task<TEntity> GetByIdAsync(string id);

        List<TEntity> GetByParameter(DocumentParameter parameter);

        Task<List<TEntity>> GetByParameterAsync(DocumentParameter parameter);

        TEntity Insert(TEntity entity);

        Task<TEntity> InsertAsync(TEntity entity);

        TEntity Update(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        void Delete(TEntity entity);

        void Delete(string id);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(string id);
    }

    public interface IBaseEntity
    {
        string Id { get; set; }
    }

}
