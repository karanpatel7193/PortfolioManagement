using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Threading.Tasks;

namespace CommonLibrary.SqlDB
{
    public class MsSql : AbstractCommonSql
    {
        #region Private Variable
        SqlConnection sqlConn;
        SqlCommand sqlCmd;
        SqlTransaction sqlTran;
        SqlDependency dependency;
        #endregion

        #region Constructor
        public MsSql()
        {
        }

        public MsSql(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public MsSql(string connectionString, string schema)
        {
            ConnectionString = connectionString;
            Schema = schema;
        }
        #endregion

        #region Sync Methods
        #region Connection Methods
        protected override void OpenConnection()
        {
            try
            {
                if (sqlConn == null || sqlConn.State != ConnectionState.Open)
                {
                    sqlConn = new SqlConnection();
                    sqlConn.ConnectionString = ConnectionString;
                    sqlConn.Open();
                }
            }
            catch (Exception ex)
            {
                sqlConn.Close();
                throw ex;
            }
        }

        protected override void CloseConnection()
        {
            try
            {
                if (sqlConn != null && sqlConn.State != ConnectionState.Closed)
                {
                    sqlConn.Close();
                }
            }
            finally
            {
                sqlConn.Dispose();
                sqlConn = null;
            }
        }

        protected override void SetCommanProperties(string commandText, CommandType commandType)
        {
            if (commandType != CommandType.Text)
                sqlCmd.CommandText = Schema + commandText;
            else
                sqlCmd.CommandText = commandText;
            sqlCmd.CommandType = commandType;
            sqlCmd.CommandTimeout = CommandTimeout;
            foreach (SqlParameter parameter in sqlParameters)
            {
                System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter("@" + parameter.ParameterName, parameter.ParameterValue);
                sqlParameter.DbType = parameter.DBType;
                sqlParameter.Direction = parameter.Direction;
                sqlCmd.Parameters.Add(sqlParameter);
            }
            sqlParameters.Clear();
        }
        #endregion

        #region Single Transaction Methods
        public override void ExecuteNonQuery(string commandText, CommandType commandType)
        {
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlCmd.Connection = sqlConn;
                sqlCmd.ExecuteNonQuery();
            }
            finally
            {
                sqlCmd.Dispose();
                CloseConnection();
            }
        }

        public override void ExecuteNonQueryWithTransaction(string commandText, CommandType commandType)
        {
            int i = 0;
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlTran = sqlConn.BeginTransaction();
                sqlCmd.Connection = sqlConn;
                sqlCmd.Transaction = sqlTran;
                i = 1;
                sqlCmd.ExecuteNonQuery();
                sqlTran.Commit();
            }
            catch (Exception ex)
            {
                if (i == 1)
                    sqlTran.Rollback();
                throw ex;
            }
            finally
            {
                sqlTran.Dispose();
                sqlCmd.Dispose();
                CloseConnection();
            }
        }

        public override object ExecuteScalar(string commandText, CommandType commandType)
        {
            object result = null;
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlCmd.Connection = sqlConn;
                result = sqlCmd.ExecuteScalar();
            }
            finally
            {
                sqlCmd.Dispose();
                CloseConnection();
            }
            return result;
        }

        public override object ExecuteScalarWithTransaction(string commandText, CommandType commandType)
        {
            object result = null;
            int i = 0;
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlTran = sqlConn.BeginTransaction();
                sqlCmd.Connection = sqlConn;
                sqlCmd.Transaction = sqlTran;
                i = 1;
                result = sqlCmd.ExecuteScalar();
                sqlTran.Commit();
            }
            catch (Exception ex)
            {
                if (i == 1)
                    sqlTran.Rollback();
                throw ex;
            }
            finally
            {
                sqlTran.Dispose();
                sqlCmd.Dispose();
                CloseConnection();
            }
            return result;
        }

        public override DataTable ExecuteDataTable(string commandText, CommandType commandType)
        {
            DataTable dtReturn = new DataTable();
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlCmd.Connection = sqlConn;
                SqlDataAdapter sqlAdp = new SqlDataAdapter(sqlCmd);
                sqlAdp.Fill(dtReturn);
                sqlAdp.Dispose();
            }
            finally
            {
                sqlCmd.Dispose();
                CloseConnection();
            }
            return dtReturn;
        }

        public override DataSet ExecuteDataSet(string commandText, CommandType commandType)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlCmd.Connection = sqlConn;
                SqlDataAdapter sqlAdp = new SqlDataAdapter(sqlCmd);
                sqlAdp.Fill(dsReturn);
                sqlAdp.Dispose();
            }
            finally
            {
                sqlCmd.Dispose();
                CloseConnection();
            }
            return dsReturn;
        }

        public override IEnumerable<IDataReader> ExecuteEnumerableDataReader(string commandText, CommandType commandType)
        {
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                        yield return reader;
                }
            }
            finally
            {
                sqlCmd.Dispose();
                CloseConnection();
            }
        }

        public override IDataReader ExecuteDataReader(string commandText, CommandType commandType)
        {
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlCmd.Connection = sqlConn;
                return sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            finally
            {
                sqlCmd.Dispose();
            }
        }

        public override List<ResultSet> ExecuteDyanamicListMultiple(string commandText, CommandType commandType)
        {
            List<ResultSet> Results = new List<ResultSet>();
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = sqlCmd.ExecuteReader())
                {
                    int index = 0;
                    do
                    {
                        ResultSet Result = new ResultSet();
                        Result.ResultIndex = index;
                        while (reader.Read())
                        {
                            IDictionary<string, object> expando = new ExpandoObject();
                            for (int i = 0; i < reader.FieldCount; i++)
                                expando.Add(reader.GetName(i), reader.GetValue(i));
                            Result.ResultData.Add(expando);
                        }
                        index++;
                        Results.Add(Result);
                    }
                    while (reader.NextResult());
                }
            }
            finally
            {
                sqlCmd.Dispose();
                CloseConnection();
            }
            return Results;
        }

        public override List<dynamic> ExecuteDyanamicList(string commandText, CommandType commandType)
        {
            List<dynamic> Results = new List<dynamic>();
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        IDictionary<string, object> expando = new ExpandoObject();
                        for (int i = 0; i < reader.FieldCount; i++)
                            expando.Add(reader.GetName(i), reader.GetValue(i));
                        Results.Add(expando);
                    }
                }
            }
            finally
            {
                sqlCmd.Dispose();
                CloseConnection();
            }
            return Results;
        }

        public override T ExecuteRecord<T>(string commandText, CommandType commandType)
        {
            object entity = Activator.CreateInstance(typeof(T));
            try
            {
                int i = 0;
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read() && i < 1)
                    {
                        entity = MapDataDynamically<T>(reader);
                        i++;
                    }
                }
            }
            finally
            {
                sqlCmd.Dispose();
                CloseConnection();
            }
            return (T)entity;
        }

        public override T ExecuteRecord<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName)
        {
            object entity = Activator.CreateInstance(typeof(T));
            try
            {
                int i = 0;
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read() && i < 1)
                    {
                        entity = mapDataFunctionName(reader);
                        i++;
                    }
                }
            }
            finally
            {
                sqlCmd.Dispose();
                CloseConnection();
            }
            return (T)entity;
        }

        public override IEnumerable<T> ExecuteEnumerable<T>(string commandText, CommandType commandType)
        {
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                        yield return MapDataDynamically<T>(reader);
                }
            }
            finally
            {
                sqlCmd.Dispose();
                CloseConnection();
            }
        }

        public override IEnumerable<T> ExecuteEnumerable<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName)
        {
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = sqlCmd.ExecuteReader())
                {
                    while (reader.Read())
                        yield return mapDataFunctionName(reader);
                }
            }
            finally
            {
                sqlCmd.Dispose();
                CloseConnection();
            }
        }

        public override void ExecuteEnumerableMultiple<T>(string commandText, CommandType commandType, int resultSetCount, T oRef, MapDataFunction<T> mapDataFunctionName)
        {
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = sqlCmd.ExecuteReader())
                {
                    for (int resultSet = 0; resultSet < resultSetCount; resultSet++)
                    {
                        while (reader.Read())
                            mapDataFunctionName(resultSet, oRef, reader);
                        reader.NextResult();
                    }
                }
            }
            finally
            {
                sqlCmd.Dispose();
                CloseConnection();
            }
        }

        public override List<T> ExecuteList<T>(string commandText, CommandType commandType)
        {
            List<T> oLists = new List<T>();
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                        oLists.Add(MapDataDynamically<T>(reader));
                }
            }
            finally
            {
                sqlCmd.Dispose();
                CloseConnection();
            }
            return oLists;
        }

        public override List<T> ExecuteList<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName)
        {
            List<T> oLists = new List<T>();
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = sqlCmd.ExecuteReader())
                {
                    while (reader.Read())
                        oLists.Add(mapDataFunctionName(reader));
                }
            }
            finally
            {
                sqlCmd.Dispose();
                CloseConnection();
            }
            return oLists;
        }

        #endregion

        #region Multiple Transaction Methods
        public override void BeginTransaction()
        {
            OpenConnection();
            sqlTran = sqlConn.BeginTransaction();
            sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConn;
            sqlCmd.Transaction = sqlTran;
        }

        public override void CommitTransaction()
        {
            try
            {
                sqlTran.Commit();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                throw ex;
            }
            finally
            {
                sqlTran.Dispose();
                sqlCmd.Dispose();
                CloseConnection();
            }
        }

        public override void RollbackTransaction()
        {
            try
            {
                sqlTran.Rollback();
            }
            finally
            {
                sqlTran.Dispose();
                sqlCmd.Dispose();
                CloseConnection();
            }
        }

        public override void ExecuteNonQueryMultipleTransaction(string commandText, CommandType commandType)
        {
            SetCommanProperties(commandText, commandType);
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Parameters.Clear();
        }

        public override object ExecuteScalarMultipleTransaction(string commandText, CommandType commandType)
        {
            object result = null;
            SetCommanProperties(commandText, commandType);
            result = sqlCmd.ExecuteScalar();
            sqlCmd.Parameters.Clear();
            return result;
        }
        #endregion
        #endregion

        #region Async Methods

        #region Async Connection Methods
        protected override async Task OpenConnectionAsync()
        {
            try
            {
                if (sqlConn == null || sqlConn.State != ConnectionState.Open)
                {
                    sqlConn = new SqlConnection();
                    sqlConn.ConnectionString = ConnectionString;
                    await sqlConn.OpenAsync();
                }
            }
            catch (Exception ex)
            {
                await sqlConn.CloseAsync();
                throw ex;
            }
        }

        protected override async Task CloseConnectionAsync()
        {
            try
            {
                if (sqlConn != null && sqlConn.State != ConnectionState.Closed)
                {
                    await sqlConn.CloseAsync();
                }
            }
            finally
            {
                await sqlConn.DisposeAsync();
                sqlConn = null;
            }
        }

        #endregion

        #region Async Single Trasaction Methods
        public override async Task ExecuteNonQueryAsync(string commandText, CommandType commandType)
        {
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                await sqlCmd.ExecuteNonQueryAsync();
            }
            finally
            {
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
        }

        public override async Task ExecuteNonQueryWithTransactionAsync(string commandText, CommandType commandType)
        {
            int i = 0;
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlTran = (SqlTransaction)await sqlConn.BeginTransactionAsync();
                sqlCmd.Connection = sqlConn;
                sqlCmd.Transaction = sqlTran;
                i = 1;
                await sqlCmd.ExecuteNonQueryAsync();
                await sqlTran.CommitAsync();
            }
            catch (Exception ex)
            {
                if (i == 1)
                    await sqlTran.RollbackAsync();
                throw ex;
            }
            finally
            {
                await sqlTran.DisposeAsync();
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
        }

        public override async Task<object> ExecuteScalarAsync(string commandText, CommandType commandType)
        {
            object result = null;
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                result = await sqlCmd.ExecuteScalarAsync();
            }
            finally
            {
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
            return result;
        }

        public override async Task<object> ExecuteScalarWithTransactionAsync(string commandText, CommandType commandType)
        {
            object result = null;
            int i = 0;
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlTran = (SqlTransaction)await sqlConn.BeginTransactionAsync();
                sqlCmd.Connection = sqlConn;
                sqlCmd.Transaction = sqlTran;
                i = 1;
                result = await sqlCmd.ExecuteScalarAsync();
                await sqlTran.CommitAsync();
            }
            catch (Exception ex)
            {
                if (i == 1)
                    await sqlTran.RollbackAsync();
                throw ex;
            }
            finally
            {
                await sqlTran.DisposeAsync();
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
            return result;
        }

        public override async Task<DataTable> ExecuteDataTableAsync(string commandText, CommandType commandType)
        {
            DataTable dtReturn = new DataTable();
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                SqlDataAdapter sqlAdp = new SqlDataAdapter(sqlCmd);
                sqlAdp.Fill(dtReturn);
                sqlAdp.Dispose();
            }
            finally
            {
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
            return dtReturn;
        }

        public override async Task<DataSet> ExecuteDataSetAsync(string commandText, CommandType commandType)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                SqlDataAdapter sqlAdp = new SqlDataAdapter(sqlCmd);
                sqlAdp.Fill(dsReturn);
                sqlAdp.Dispose();
            }
            finally
            {
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
            return dsReturn;
        }

        public override async IAsyncEnumerable<IDataReader> ExecuteEnumerableDataReaderAsync(string commandText, CommandType commandType)
        {
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = await sqlCmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                        yield return reader;
                }
            }
            finally
            {
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
        }

        public override async Task<IDataReader> ExecuteDataReaderAsync(string commandText, CommandType commandType)
        {
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                return await sqlCmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
            finally
            {
                await sqlCmd.DisposeAsync();
            }
        }

        public override async Task<List<ResultSet>> ExecuteDyanamicListMultipleAsync(string commandText, CommandType commandType)
        {
            List<ResultSet> Results = new List<ResultSet>();
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = await sqlCmd.ExecuteReaderAsync())
                {
                    int index = 0;
                    do
                    {
                        ResultSet Result = new ResultSet();
                        Result.ResultIndex = index;
                        while (reader.Read())
                        {
                            IDictionary<string, object> expando = new ExpandoObject();
                            for (int i = 0; i < reader.FieldCount; i++)
                                expando.Add(reader.GetName(i), reader.GetValue(i));
                            Result.ResultData.Add(expando);
                        }
                        index++;
                        Results.Add(Result);
                    }
                    while (reader.NextResult());
                }
            }
            finally
            {
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
            return Results;
        }

        public override async Task<List<dynamic>> ExecuteDyanamicListAsync(string commandText, CommandType commandType)
        {
            List<dynamic> Results = new List<dynamic>();
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = await sqlCmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        IDictionary<string, object> expando = new ExpandoObject();
                        for (int i = 0; i < reader.FieldCount; i++)
                            expando.Add(reader.GetName(i), reader.GetValue(i));
                        Results.Add(expando);
                    }
                }
            }
            finally
            {
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
            return Results;
        }

        public override async Task<T> ExecuteRecordAsync<T>(string commandText, CommandType commandType)
        {
            object entity = Activator.CreateInstance(typeof(T));
            try
            {
                int i = 0;
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = await sqlCmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (reader.Read() && i < 1)
                    {
                        entity = await MapDataDynamicallyAsync<T>(reader);
                        i++;
                    }
                }
            }
            finally
            {
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
            return (T)entity;
        }

        public override async Task<T> ExecuteRecordAsync<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName)
        {
            object entity = Activator.CreateInstance(typeof(T));
            try
            {
                int i = 0;
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = await sqlCmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (reader.Read() && i < 1)
                    {
                        entity = mapDataFunctionName(reader);
                        i++;
                    }
                }
            }
            finally
            {
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
            return (T)entity;
        }

        public override async IAsyncEnumerable<T> ExecuteEnumerableAsync<T>(string commandText, CommandType commandType)
        {
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = await sqlCmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                        yield return await MapDataDynamicallyAsync<T>(reader);
                }
            }
            finally
            {
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
        }

        public override async IAsyncEnumerable<T> ExecuteEnumerableAsync<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName)
        {
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = await sqlCmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                        yield return mapDataFunctionName(reader);
                }
            }
            finally
            {
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
        }

        public override async Task ExecuteEnumerableMultipleAsync<T>(string commandText, CommandType commandType, int resultSetCount, T oRef, MapDataFunctionAsync<T> mapDataFunctionNameAsync)
        {
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = await sqlCmd.ExecuteReaderAsync())
                {
                    for (int resultSet = 0; resultSet < resultSetCount; resultSet++)
                    {
                        while (reader.Read())
                            await mapDataFunctionNameAsync(resultSet, oRef, reader);
                        reader.NextResult();
                    }
                }
            }
            finally
            {
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
        }

        public override async Task<List<T>> ExecuteListAsync<T>(string commandText, CommandType commandType)
        {
            List<T> oLists = new List<T>();
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = await sqlCmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                        oLists.Add(await MapDataDynamicallyAsync<T>(reader));
                }
            }
            finally
            {
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
            return oLists;
        }

        public override async Task<List<T>> ExecuteListAsync<T>(string commandText, CommandType commandType, Func<IDataReader, T> mapDataFunctionName)
        {
            List<T> oLists = new List<T>();
            try
            {
                sqlCmd = new SqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = await sqlCmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                        oLists.Add(mapDataFunctionName(reader));
                }
            }
            finally
            {
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
            return oLists;
        }

        #endregion

        #region Multiple Transaction Methods
        public override async Task BeginTransactionAsync()
        {
            await OpenConnectionAsync();
            sqlTran = (SqlTransaction)await sqlConn.BeginTransactionAsync();
            sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConn;
            sqlCmd.Transaction = sqlTran;
        }

        public override async Task CommitTransactionAsync()
        {
            try
            {
                await sqlTran.CommitAsync();
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync();
                throw ex;
            }
            finally
            {
                await sqlTran.DisposeAsync();
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
        }

        public override async Task RollbackTransactionAsync()
        {
            try
            {
                await sqlTran.RollbackAsync();
            }
            finally
            {
                await sqlTran.DisposeAsync();
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
        }

        public override async Task ExecuteNonQueryMultipleTransactionAsync(string commandText, CommandType commandType)
        {
            SetCommanProperties(commandText, commandType);
            await sqlCmd.ExecuteNonQueryAsync();
            sqlCmd.Parameters.Clear();
        }

        public override async Task<object> ExecuteScalarMultipleTransactionAsync(string commandText, CommandType commandType)
        {
            object result = null;
            SetCommanProperties(commandText, commandType);
            result = await sqlCmd.ExecuteScalarAsync();
            sqlCmd.Parameters.Clear();
            return result;
        }
        #endregion
        #endregion

        #region SqlNotification
        public override void SqlNotificationStart()
        {
            SqlDependency.Start(ConnectionString);
        }

        public override void SqlNotificationEnd()
        {
            SqlDependency.Stop(ConnectionString);
        }

        public override void SqlNotificationDeregisterEvent(SqlNotificationOnSend sqlNotificationOnSend)
        {
            dependency.OnChange -= new OnChangeEventHandler(sqlNotificationOnSend);
        }

        public override List<T> SqlNotification<T>(string commandText, CommandType commandType, SqlNotificationOnSend sqlNotificationOnSend)
        {
            List<T> lists = new List<T>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    if (commandType != CommandType.Text)
                        command.CommandText = Schema + commandText;
                    else
                        command.CommandText = commandText;
                    command.CommandType = commandType;
                    foreach (SqlParameter parameter in sqlParameters)
                    {
                        System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter("@" + parameter.ParameterName, parameter.ParameterValue);
                        sqlParameter.DbType = parameter.DBType;
                        sqlParameter.Direction = parameter.Direction;
                        command.Parameters.Add(sqlParameter);
                    }
                    sqlParameters.Clear();
                    command.Connection = connection;
                    command.Notification = null;

                    dependency = new SqlDependency(command);
                    if (connection.State == System.Data.ConnectionState.Closed)
                        connection.Open();

                    dependency.OnChange += new OnChangeEventHandler(sqlNotificationOnSend);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            lists.Add(MapDataDynamically<T>(reader));
                    }
                }
            }
            return lists;
        }

        public override List<T> SqlNotification<T>(string table, string columns, SqlNotificationOnSend sqlNotificationOnSend)
        {
            List<T> lists = new List<T>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = string.Format("SELECT {0} FROM {2}{1}", columns, table, Schema.Length == 0 ? "dbo." : Schema);
                    command.CommandType = CommandType.Text;
                    command.Connection = connection;
                    command.Notification = null;

                    dependency = new SqlDependency(command);
                    if (connection.State == System.Data.ConnectionState.Closed)
                        connection.Open();

                    dependency.OnChange += new OnChangeEventHandler(sqlNotificationOnSend);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            lists.Add(MapDataDynamically<T>(reader));
                    }
                }
            }
            return lists;
        }
        #endregion
    }
}
