using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommonLibrary.DocumentDB
{
    public class Mongo<TEntity> : IDocument<TEntity> where TEntity : IBaseEntity
    {
        #region Variable & Constructor
        private readonly IMongoCollection<TEntity> _collection;

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string CollectionName { get; set; }

        public Mongo()
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(DatabaseName);
            _collection = database.GetCollection<TEntity>(CollectionName);
        }

        public Mongo(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<TEntity>(collectionName);
        }
        #endregion

        #region Public Methods
        public List<TEntity> GetAll()
        {
            return _collection.Find(document => true).ToList();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            var result = await _collection.FindAsync(document => true);
            return result.ToList();
        }

        public TEntity GetById(string id)
        {
            return _collection.Find<TEntity>(document => document.Id == id).FirstOrDefault();
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            var result = await _collection.FindAsync<TEntity>(document => document.Id == id);
            return result.SingleOrDefault();
        }

        public List<TEntity> GetByParameter(DocumentParameter parameter)
        {
            return _collection.Find(GetSearchCriteriaString(parameter)).ToList();
        }

        public async Task<List<TEntity>> GetByParameterAsync(DocumentParameter parameter)
        {
            //List<TEntity> entities = new List<TEntity>();
            //var filter = GetSearchCriteriaString(parameter); //"{ FirstName: 'Peter'}";
            //await _collection.Find(filter).ForEachAsync<TEntity>(document => entities.Add(document));
            //return entities;

            var result = await _collection.FindAsync(GetSearchCriteriaString(parameter));
            return result.ToList();

        }

        public TEntity Insert(TEntity entity)
        {
            _collection.InsertOne(entity);
            return entity;
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            _collection.ReplaceOne(document => document.Id == entity.Id, entity);
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await _collection.ReplaceOneAsync(document => document.Id == entity.Id, entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
            _collection.DeleteOne(document => document.Id == entity.Id);
        }

        public void Delete(string id)
        {
            _collection.DeleteOne(document => document.Id == id);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await _collection.DeleteOneAsync(document => document.Id == entity.Id);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(document => document.Id == id);
        }
        #endregion

        #region Private Methods
        private string GetSearchCriteriaString(DocumentParameter parameter)
        {
            if (parameter != null && (parameter.Childs.Count > 0 || parameter.Parameter != string.Empty))
                return GetSearchCriteriaProcessString(parameter);
            else
                return string.Empty;
        }

        private string GetSearchCriteriaProcessString(DocumentParameter parameter)
        {
            string searchCriteria = string.Empty;
            if (parameter.Childs.Count == 0 && parameter.Parameter != string.Empty)
                searchCriteria += GetSingleSearchString(parameter);
            else
            {
                searchCriteria += GetSearchLogicalGroupString(parameter.Childs, parameter.Condition, GroupOperator.AND, "$and");
                searchCriteria += GetSearchLogicalGroupString(parameter.Childs, parameter.Condition, GroupOperator.NOT, "$not");
                searchCriteria += GetSearchLogicalGroupString(parameter.Childs, parameter.Condition, GroupOperator.NOR, "$nor");
                searchCriteria += GetSearchLogicalGroupString(parameter.Childs, parameter.Condition, GroupOperator.OR, "$or");
            }
            return searchCriteria;
        }

        private string GetSearchLogicalGroupString(List<DocumentParameter> parameters, GroupOperator condition, GroupOperator groupOperator, string groupOperatorString)
        {
            string searchCriteria = "";

            if (condition == groupOperator)
            {
                searchCriteria += "{ " + groupOperatorString + " : [ ";

                for (int criteriaIndex = 0; criteriaIndex < parameters.Count; criteriaIndex++)
                {
                    searchCriteria += GetSingleSearchString(parameters[criteriaIndex]) + ",";
                }
                searchCriteria = searchCriteria.TrimEnd(',') + " ] }";

            }
            return searchCriteria;
        }

        private string GetSingleSearchString(DocumentParameter parameter)
        {
            string searchCriteria = "";
            if (parameter.Childs.Count > 0)
                searchCriteria += GetSearchCriteriaProcessString(parameter);

            if (parameter.Parameter != string.Empty)
            {
                if (parameter.Compare == CompareOperator.Equal)
                    searchCriteria += "{ " + parameter.Parameter + " : { $eq: " + parameter.DocumentValue + " } } ";
                else if (parameter.Compare == CompareOperator.NotEqual)
                    searchCriteria += "{ " + parameter.Parameter + " : { $ne: " + parameter.DocumentValue + " } } ";
                else if (parameter.Compare == CompareOperator.GreaterThan)
                    searchCriteria += "{ " + parameter.Parameter + " : { $gt: " + parameter.DocumentValue + " } } ";
                else if (parameter.Compare == CompareOperator.GreaterThanEqual)
                    searchCriteria += "{ " + parameter.Parameter + " : { $gte: " + parameter.DocumentValue + " } } ";
                else if (parameter.Compare == CompareOperator.LessThan)
                    searchCriteria += "{ " + parameter.Parameter + " : { $lt: " + parameter.DocumentValue + " } } ";
                else if (parameter.Compare == CompareOperator.LessThanEqual)
                    searchCriteria += "{ " + parameter.Parameter + " : { $lte: " + parameter.DocumentValue + " } } ";
                else if (parameter.Compare == CompareOperator.Contains)
                    searchCriteria += "{ " + parameter.Parameter + " : { $regex: /.*" + parameter.Value + ".*/, $options: 'im' } } ";
                else if (parameter.Compare == CompareOperator.BeginWith)
                    searchCriteria += "{ " + parameter.Parameter + " : { $regex: /^" + parameter.Value + "/, $options: 'im' } } ";
                else if (parameter.Compare == CompareOperator.EndWith)
                    searchCriteria += "{ " + parameter.Parameter + " : { $regex: /" + parameter.Value + "$/, $options: 'im' } } ";
                else if (parameter.Compare == CompareOperator.In)
                    searchCriteria += "{ " + parameter.Parameter + " : { $in: " + parameter.Value + " } } ";
                else if (parameter.Compare == CompareOperator.NotIn)
                    searchCriteria += "{ " + parameter.Parameter + " : { $nin: " + parameter.Value + " } } ";
            }
            return searchCriteria;
        }

        #endregion
    }

    public class BaseEntity : IBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

    }
}
