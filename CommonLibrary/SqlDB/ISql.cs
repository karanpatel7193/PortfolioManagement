using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CommonLibrary.SqlDB
{
    public interface ISql
    {
        int CommandTimeout { get; set; }
        string Schema { get; set; }

        void ExecuteNonQuery(string commandText, CommandType commandType);
        void ExecuteNonQueryWithTransaction(string commandText, CommandType commandType);
        object ExecuteScalar(string commandText, CommandType commandType);
        object ExecuteScalarWithTransaction(string commandText, CommandType commandType);

        DataTable ExecuteDataTable(string commandText, CommandType commandType);
        DataSet ExecuteDataSet(string commandText, CommandType commandType);
        IEnumerable<IDataReader> ExecuteEnumerableDataReader(string commandText, CommandType commandType);
        IDataReader ExecuteDataReader(string commandText, CommandType commandType);
        List<ResultSet> ExecuteDyanamicListMultiple(string commandText, CommandType commandType);
        List<dynamic> ExecuteDyanamicList(string commandText, CommandType commandType);
        T ExecuteRecord<T>(string commandText, CommandType commandType);
        T ExecuteRecord<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName);

        IEnumerable<T> ExecuteEnumerable<T>(string commandText, CommandType commandType);
        IEnumerable<T> ExecuteEnumerable<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName);
        void ExecuteEnumerableMultiple<T>(string commandText, CommandType commandType, int resultSetCount, T oRef, MapDataFunction<T> mapDataFunctionName);

        List<T> ExecuteList<T>(string commandText, CommandType commandType);
        List<T> ExecuteList<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName);

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();

        void ExecuteNonQueryMultipleTransaction(string commandText, CommandType commandType);
        object ExecuteScalarMultipleTransaction(string commandText, CommandType commandType);

        void AddParameter(string parameterName, object parameterValue);
        void AddParameter(string parameterName, DbType dbType, ParameterDirection direction, object parameterValue);
        void ClearParameter();
        Dictionary<string, object> GetParameters();
        List<T> FillList<T>(IDataReader reader);
        List<T> FillList<T>(IDataReader reader, Func<IDataReader, T> mapDataFunctionName);

        T MapDataDynamically<T>(IDataReader reader);

        #region Async Methods
        Task ExecuteNonQueryAsync(string commandText, CommandType commandType);

        Task ExecuteNonQueryWithTransactionAsync(string commandText, CommandType commandType);

        Task<object> ExecuteScalarAsync(string commandText, CommandType commandType);

        Task<object> ExecuteScalarWithTransactionAsync(string commandText, CommandType commandType);

        Task<DataTable> ExecuteDataTableAsync(string commandText, CommandType commandType);

        Task<DataSet> ExecuteDataSetAsync(string commandText, CommandType commandType);

        IAsyncEnumerable<IDataReader> ExecuteEnumerableDataReaderAsync(string commandText, CommandType commandType);

        Task<IDataReader> ExecuteDataReaderAsync(string commandText, CommandType commandType);

        Task<List<ResultSet>> ExecuteDyanamicListMultipleAsync(string commandText, CommandType commandType);

        Task<List<dynamic>> ExecuteDyanamicListAsync(string commandText, CommandType commandType);

        Task<T> ExecuteRecordAsync<T>(string commandText, CommandType commandType);

        Task<T> ExecuteRecordAsync<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName);

        IAsyncEnumerable<T> ExecuteEnumerableAsync<T>(string commandText, CommandType commandType);

        IAsyncEnumerable<T> ExecuteEnumerableAsync<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName);

        Task ExecuteEnumerableMultipleAsync<T>(string commandText, CommandType commandType, int resultSetCount, T oRef, MapDataFunctionAsync<T> mapDataFunctionName);

        Task<List<T>> ExecuteListAsync<T>(string commandText, CommandType commandType);

        Task<List<T>> ExecuteListAsync<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName);

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();

        Task ExecuteNonQueryMultipleTransactionAsync(string commandText, CommandType commandType);

        Task<object> ExecuteScalarMultipleTransactionAsync(string commandText, CommandType commandType);

        Task<List<T>> FillListAsync<T>(IDataReader reader);

        Task<T> MapDataDynamicallyAsync<T>(IDataReader reader);

        #endregion


        #region SqlNotification
        void SqlNotificationStart();
        void SqlNotificationEnd();
        void SqlNotificationDeregisterEvent(SqlNotificationOnSend sqlNotificationOnSend);
        List<T> SqlNotification<T>(string commandText, CommandType commandType, SqlNotificationOnSend sqlNotificationOnSend);
        List<T> SqlNotification<T>(string table, string columns, SqlNotificationOnSend sqlNotificationOnSend);
        #endregion

    }
}
