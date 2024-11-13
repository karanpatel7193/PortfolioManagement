using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

namespace CommonLibrary.SqlDB
{
    //public delegate T MapDataFunction<T>(IDataReader Reader, int ResultSet);
    public delegate void MapDataFunction<T>(int resultSet, T oEntityRef, IDataReader reader);
    public delegate Task MapDataFunctionAsync<T>(int resultSet, T oEntityRef, IDataReader reader);
    public delegate void SqlNotificationOnSend(object sender, System.Data.SqlClient.SqlNotificationEventArgs e);

    public abstract class AbstractCommonSql : ISql
    {
        #region Private Variable
        private string _schema;
        private string _connectionString;

        protected List<SqlParameter> sqlParameters = new List<SqlParameter>();
        #endregion

        #region Public Properties
        /// <summary>
        /// Get & Set Connection String
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        private int _CommandTimeout = 30;
        public int CommandTimeout
        {
            get
            {
                return _CommandTimeout;
            }
            set
            {
                _CommandTimeout = value;
            }
        }
        /// <summary>
        /// Get & Set Connection String
        /// </summary>
        public string Schema
        {
            get
            {
                if (MyConvert.ToString(_schema) != string.Empty)
                    return _schema + ".";
                else
                    return string.Empty;
            }
            set { _schema = value; }
        }
        #endregion

        #region Abstract Methods
        protected abstract void OpenConnection();
        protected abstract void CloseConnection();
        protected abstract void SetCommanProperties(string commandText, CommandType commandType);

        public abstract void ExecuteNonQuery(string commandText, CommandType commandType);
        public abstract void ExecuteNonQueryWithTransaction(string commandText, CommandType commandType);
        public abstract object ExecuteScalar(string commandText, CommandType commandType);
        public abstract object ExecuteScalarWithTransaction(string commandText, CommandType commandType);

        public abstract DataTable ExecuteDataTable(string commandText, CommandType commandType);
        public abstract DataSet ExecuteDataSet(string commandText, CommandType commandType);
        public abstract IEnumerable<IDataReader> ExecuteEnumerableDataReader(string commandText, CommandType commandType);
        public abstract IDataReader ExecuteDataReader(string commandText, CommandType commandType);
        public abstract List<ResultSet> ExecuteDyanamicListMultiple(string commandText, CommandType commandType);
        public abstract List<dynamic> ExecuteDyanamicList(string commandText, CommandType commandType);

        public abstract T ExecuteRecord<T>(string commandText, CommandType commandType);
        public abstract T ExecuteRecord<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName);

        public abstract List<T> ExecuteList<T>(string commandText, CommandType commandType);
        public abstract List<T> ExecuteList<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName);

        public abstract IEnumerable<T> ExecuteEnumerable<T>(string commandText, CommandType commandType);
        public abstract IEnumerable<T> ExecuteEnumerable<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName);
        public abstract void ExecuteEnumerableMultiple<T>(string commandText, CommandType commandType, int resultSetCount, T oRef, MapDataFunction<T> mapDataFunctionName);

        public abstract void BeginTransaction();
        public abstract void CommitTransaction();
        public abstract void RollbackTransaction();

        public abstract void ExecuteNonQueryMultipleTransaction(string commandText, CommandType commandType);
        public abstract object ExecuteScalarMultipleTransaction(string commandText, CommandType commandType);
        #endregion

        #region Public Methods
        public void AddParameter(string parameterName, object parameterValue)
        {
            if (parameterValue == null)
                parameterValue = DBNull.Value;
            sqlParameters.Add(new SqlParameter(parameterName, parameterValue));
        }
        public void AddParameter(string parameterName, DbType dbType, ParameterDirection direction, object parameterValue)
        {
            if (parameterValue == null)
                parameterValue = DBNull.Value;
            sqlParameters.Add(new SqlParameter(parameterName, dbType, direction, parameterValue));
        }
        public void ClearParameter()
        {
            sqlParameters.Clear();
        }

        public Dictionary<string, object> GetParameters()
        {
            Dictionary<string, object> _dict = new Dictionary<string, object>();
            sqlParameters.ForEach(d => _dict.Add(d.ParameterName, d.ParameterValue));
            return _dict;
        }

        public T MapDataDynamically<T>(IDataReader reader)
        {
            object entity = Activator.CreateInstance(typeof(T));

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader[reader.GetName(i)] != DBNull.Value)
                {
                    PropertyInfo propertyInfo = entity.GetType().GetProperty(reader.GetName(i));
                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(entity, reader[reader.GetName(i)], null);
                    }
                    else
                    {
                        PropertyInfo propertyInfoInsensitive = entity.GetType().GetProperty(reader.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (propertyInfoInsensitive != null)
                        {
                            propertyInfoInsensitive.SetValue(entity, reader[reader.GetName(i)], null);
                        }
                    }
                }
            }
            return (T)entity;
        }

        public List<T> FillList<T>(IDataReader reader)
        {
            List<T> oLists = new List<T>();
            while (reader.Read())
                oLists.Add(MapDataDynamically<T>(reader));
            reader.NextResult();
            return oLists;
        }

        public List<T> FillList<T>(IDataReader reader, Func<IDataReader, T> mapDataFunctionName)
        {
            List<T> oLists = new List<T>();
            while (reader.Read())
                oLists.Add(mapDataFunctionName(reader));
            reader.NextResult();
            return oLists;
        }
        #endregion

        #region Private Methods

        #endregion

        #region Async Methods
        protected abstract Task OpenConnectionAsync();

        protected abstract Task CloseConnectionAsync();

        public abstract Task ExecuteNonQueryAsync(string commandText, CommandType commandType);

        public abstract Task ExecuteNonQueryWithTransactionAsync(string commandText, CommandType commandType);

        public abstract Task<object> ExecuteScalarAsync(string commandText, CommandType commandType);

        public abstract Task<object> ExecuteScalarWithTransactionAsync(string commandText, CommandType commandType);

        public abstract Task<DataTable> ExecuteDataTableAsync(string commandText, CommandType commandType);

        public abstract IAsyncEnumerable<IDataReader> ExecuteEnumerableDataReaderAsync(string commandText, CommandType commandType);

        public abstract Task<IDataReader> ExecuteDataReaderAsync(string commandText, CommandType commandType);

        public abstract Task<List<ResultSet>> ExecuteDyanamicListMultipleAsync(string commandText, CommandType commandType);

        public abstract Task<List<dynamic>> ExecuteDyanamicListAsync(string commandText, CommandType commandType);

        public abstract Task<T> ExecuteRecordAsync<T>(string commandText, CommandType commandType);

        public abstract Task<T> ExecuteRecordAsync<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName);

        public abstract IAsyncEnumerable<T> ExecuteEnumerableAsync<T>(string commandText, CommandType commandType);

        public abstract IAsyncEnumerable<T> ExecuteEnumerableAsync<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName);

        public abstract Task ExecuteEnumerableMultipleAsync<T>(string commandText, CommandType commandType, int resultSetCount, T oRef, MapDataFunctionAsync<T> mapDataFunctionNameAsync);

        public abstract Task<List<T>> ExecuteListAsync<T>(string commandText, CommandType commandType);

        public abstract Task<List<T>> ExecuteListAsync<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName);

        public abstract Task BeginTransactionAsync();

        public abstract Task CommitTransactionAsync();

        public abstract Task RollbackTransactionAsync();

        public abstract Task ExecuteNonQueryMultipleTransactionAsync(string commandText, CommandType commandType);

        public abstract Task<object> ExecuteScalarMultipleTransactionAsync(string commandText, CommandType commandType);

        public Task<T> MapDataDynamicallyAsync<T>(IDataReader reader)
        {
            object entity = Activator.CreateInstance(typeof(T));

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader[reader.GetName(i)] != DBNull.Value)
                {
                    PropertyInfo propertyInfo = entity.GetType().GetProperty(reader.GetName(i));
                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(entity, reader[reader.GetName(i)], null);
                    }
                    else
                    {
                        PropertyInfo propertyInfoInsensitive = entity.GetType().GetProperty(reader.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (propertyInfoInsensitive != null)
                        {
                            propertyInfoInsensitive.SetValue(entity, reader[reader.GetName(i)], null);
                        }
                    }
                }
            }
            return Task.FromResult(((T)entity));
        }

        public async Task<List<T>> FillListAsync<T>(IDataReader reader)
        {
            List<T> oLists = new List<T>();
            while (reader.Read())
                oLists.Add(await MapDataDynamicallyAsync<T>(reader));
            reader.NextResult();
            return oLists;
        }

        public abstract Task<DataSet> ExecuteDataSetAsync(string commandText, CommandType commandType);

        #endregion

        #region SqlNotification
        public abstract void SqlNotificationStart();
        public abstract void SqlNotificationEnd();
        public abstract void SqlNotificationDeregisterEvent(SqlNotificationOnSend sqlNotificationOnSend);
        public abstract List<T> SqlNotification<T>(string commandText, CommandType commandType, SqlNotificationOnSend sqlNotificationOnSend);
        public abstract List<T> SqlNotification<T>(string table, string columns, SqlNotificationOnSend sqlNotificationOnSend);
        #endregion
    }
}
