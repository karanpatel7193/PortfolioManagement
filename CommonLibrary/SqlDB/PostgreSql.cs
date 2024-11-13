using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Threading.Tasks;

namespace CommonLibrary.SqlDB
{
    public class PostgreSql : AbstractCommonSql
    {
        #region Private Variable
        NpgsqlConnection sqlConn;
        NpgsqlCommand sqlCmd;
        NpgsqlTransaction sqlTran;
        #endregion

        #region Constructor
        public PostgreSql()
        {
        }

        public PostgreSql(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public PostgreSql(string connectionString, string schema)
        {
            ConnectionString = connectionString;
            Schema = schema;
        }
        #endregion

        #region Connection Methods
        protected override void OpenConnection()
        {
            try
            {
                if (sqlConn == null || sqlConn.State != ConnectionState.Open)
                {
                    sqlConn = new NpgsqlConnection();
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
            string Parameter = string.Empty;
            if (commandType != CommandType.Text)
                sqlCmd.CommandText = Schema + commandText.ToLower();
            else
                sqlCmd.CommandText = commandText.ToLower();
            sqlCmd.CommandType = commandType;
            sqlCmd.CommandTimeout = CommandTimeout;
            foreach (SqlParameter parameter in sqlParameters)
            {
                Parameter += "@" + parameter.ParameterName + " = '" + parameter.ParameterValue + ",";
                NpgsqlParameter sqlParameter = new NpgsqlParameter(parameter.ParameterName, parameter.ParameterValue);
                //sqlParameter.DbType = sqlParameter.DBType;
                //sqlParameter.Direction = sqlParameter.Direction;
                sqlCmd.Parameters.Add(sqlParameter);
            }
            sqlParameters.Clear();
        }
        #endregion

        #region Single Trasaction Methods
        public override void ExecuteNonQuery(string commandText, CommandType commandType)
        {
            try
            {
                sqlCmd = new NpgsqlCommand();
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
                sqlCmd = new NpgsqlCommand();
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
                if (sqlTran != null)
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
                sqlCmd = new NpgsqlCommand();
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
                sqlCmd = new NpgsqlCommand();
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
                if (sqlTran != null)
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
                sqlCmd = new NpgsqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlCmd.Connection = sqlConn;
                NpgsqlDataAdapter sqlAdp = new NpgsqlDataAdapter(sqlCmd);
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
            int i = 0;
            try
            {
                sqlCmd = new NpgsqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlTran = sqlConn.BeginTransaction();
                sqlCmd.Connection = sqlConn;
                sqlCmd.Transaction = sqlTran;
                i = 1;
                NpgsqlDataAdapter sqlAdp = new NpgsqlDataAdapter(sqlCmd);
                sqlAdp.Fill(dsReturn);
                sqlTran.Commit();
                sqlAdp.Dispose();
            }
            catch (Exception ex)
            {
                if (i == 1)
                    sqlTran.Rollback();
                throw ex;
            }
            finally
            {
                if (sqlTran != null)
                    sqlTran.Dispose();
                sqlCmd.Dispose();
                CloseConnection();
            }
            return dsReturn;
        }

        public override IEnumerable<IDataReader> ExecuteEnumerableDataReader(string commandText, CommandType commandType)
        {
            try
            {
                sqlCmd = new NpgsqlCommand();
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
                sqlCmd = new NpgsqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlTran = sqlConn.BeginTransaction();
                sqlCmd.Connection = sqlConn;
                sqlCmd.Transaction = sqlTran;
                IDataReader reader = sqlCmd.ExecuteReader();
                sqlTran.Commit();
                return reader;
            }
            catch (Exception ex)
            {
                sqlTran.Rollback();
                throw ex;
            }
            finally
            {
                sqlCmd.Dispose();
                if (sqlTran != null)
                    sqlTran.Dispose();
                CloseConnection();
            }
        }

        public override List<ResultSet> ExecuteDyanamicListMultiple(string commandText, CommandType commandType)
        {
            List<ResultSet> Results = new List<ResultSet>();
            int iTransaction = 0;
            try
            {
                sqlCmd = new NpgsqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlTran = sqlConn.BeginTransaction();
                sqlCmd.Connection = sqlConn;
                sqlCmd.Transaction = sqlTran;
                iTransaction = 1;
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
                sqlTran.Commit();
            }
            catch (Exception ex)
            {
                if (iTransaction == 1)
                    sqlTran.Rollback();
                throw ex;
            }
            finally
            {
                sqlCmd.Dispose();
                if (sqlTran != null)
                    sqlTran.Dispose();
                CloseConnection();
            }
            return Results;
        }

        public override List<dynamic> ExecuteDyanamicList(string commandText, CommandType commandType)
        {
            List<dynamic> Results = new List<dynamic>();
            try
            {
                sqlCmd = new NpgsqlCommand();
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
                sqlCmd = new NpgsqlCommand();
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
                sqlCmd = new NpgsqlCommand();
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
                sqlCmd = new NpgsqlCommand();
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
                sqlCmd = new NpgsqlCommand();
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
            int i = 0;
            try
            {
                sqlCmd = new NpgsqlCommand();
                SetCommanProperties(commandText, commandType);
                OpenConnection();
                sqlTran = sqlConn.BeginTransaction();
                sqlCmd.Connection = sqlConn;
                sqlCmd.Transaction = sqlTran;
                i = 1;
                using (IDataReader reader = sqlCmd.ExecuteReader())
                {
                    for (int resultSet = 0; resultSet < resultSetCount; resultSet++)
                    {
                        while (reader.Read())
                            mapDataFunctionName(resultSet, oRef, reader);
                        reader.NextResult();
                    }
                }
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
                sqlCmd.Dispose();
                if (sqlTran != null)
                    sqlTran.Dispose();
                CloseConnection();
            }
        }

        public override List<T> ExecuteList<T>(string commandText, CommandType commandType)
        {
            List<T> oLists = new List<T>();
            try
            {
                sqlCmd = new NpgsqlCommand();
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
                sqlCmd = new NpgsqlCommand();
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
            sqlCmd = new NpgsqlCommand();
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

        #region Async Method

        #region Connection Methods
        protected override async Task OpenConnectionAsync()
        {
            try
            {
                if (sqlConn == null || sqlConn.State != ConnectionState.Open)
                {
                    sqlConn = new NpgsqlConnection();
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

        #region Single Trasaction Methods
        public override async Task ExecuteNonQueryAsync(string commandText, CommandType commandType)
        {
            try
            {
                sqlCmd = new NpgsqlCommand();
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
                sqlCmd = new NpgsqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlTran = (NpgsqlTransaction)await sqlConn.BeginTransactionAsync();
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
                if (sqlTran != null)
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
                sqlCmd = new NpgsqlCommand();
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
                sqlCmd = new NpgsqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlTran = (NpgsqlTransaction)await sqlConn.BeginTransactionAsync();
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
                if (sqlTran != null)
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
                sqlCmd = new NpgsqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                NpgsqlDataAdapter sqlAdp = new NpgsqlDataAdapter(sqlCmd);
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
            int i = 0;
            try
            {
                sqlCmd = new NpgsqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlTran = (NpgsqlTransaction)await sqlConn.BeginTransactionAsync();
                sqlCmd.Connection = sqlConn;
                sqlCmd.Transaction = sqlTran;
                i = 1;
                NpgsqlDataAdapter sqlAdp = new NpgsqlDataAdapter(sqlCmd);
                sqlAdp.Fill(dsReturn);
                await sqlTran.CommitAsync();
                sqlAdp.Dispose();
            }
            catch (Exception ex)
            {
                if (i == 1)
                    await sqlTran.RollbackAsync();
                throw ex;
            }
            finally
            {
                if (sqlTran != null)
                    await sqlTran.DisposeAsync();
                await sqlCmd.DisposeAsync();
                await CloseConnectionAsync();
            }
            return dsReturn;
        }

        public override async IAsyncEnumerable<IDataReader> ExecuteEnumerableDataReaderAsync(string commandText, CommandType commandType)
        {
            try
            {
                sqlCmd = new NpgsqlCommand();
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
                sqlCmd = new NpgsqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlTran = (NpgsqlTransaction)await sqlConn.BeginTransactionAsync();
                sqlCmd.Connection = sqlConn;
                sqlCmd.Transaction = sqlTran;
                IDataReader reader = await sqlCmd.ExecuteReaderAsync();
                await sqlTran.CommitAsync();
                return reader;
            }
            catch (Exception ex)
            {
                await sqlTran.RollbackAsync();
                throw ex;
            }
            finally
            {
                await sqlCmd.DisposeAsync();
                if (sqlTran != null)
                    await sqlTran.DisposeAsync();
                await CloseConnectionAsync();
            }
        }

        public override async Task<List<ResultSet>> ExecuteDyanamicListMultipleAsync(string commandText, CommandType commandType)
        {
            List<ResultSet> Results = new List<ResultSet>();
            int iTransaction = 0;
            try
            {
                sqlCmd = new NpgsqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlTran = (NpgsqlTransaction)await sqlConn.BeginTransactionAsync();
                sqlCmd.Connection = sqlConn;
                sqlCmd.Transaction = sqlTran;
                iTransaction = 1;
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
                await sqlTran.CommitAsync();
            }
            catch (Exception ex)
            {
                if (iTransaction == 1)
                    await sqlTran.RollbackAsync();
                throw ex;
            }
            finally
            {
                await sqlCmd.DisposeAsync();
                if (sqlTran != null)
                    await sqlTran.DisposeAsync();
                await CloseConnectionAsync();
            }
            return Results;
        }

        public override async Task<List<dynamic>> ExecuteDyanamicListAsync(string commandText, CommandType commandType)
        {
            List<dynamic> Results = new List<dynamic>();
            try
            {
                sqlCmd = new NpgsqlCommand();
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
                sqlCmd = new NpgsqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlCmd.Connection = sqlConn;
                using (IDataReader reader = await sqlCmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (reader.Read() && i < 1)
                    {
                        entity = MapDataDynamicallyAsync<T>(reader);
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
                sqlCmd = new NpgsqlCommand();
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
                sqlCmd = new NpgsqlCommand();
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
                sqlCmd = new NpgsqlCommand();
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

        public override async Task ExecuteEnumerableMultipleAsync<T>(string commandText, CommandType commandType, int resultSetCount, T oRef, MapDataFunctionAsync<T> mapDataFunctionName)
        {
            int i = 0;
            try
            {
                sqlCmd = new NpgsqlCommand();
                SetCommanProperties(commandText, commandType);
                await OpenConnectionAsync();
                sqlTran = (NpgsqlTransaction)await sqlConn.BeginTransactionAsync();
                sqlCmd.Connection = sqlConn;
                sqlCmd.Transaction = sqlTran;
                i = 1;
                using (IDataReader reader = await sqlCmd.ExecuteReaderAsync())
                {
                    for (int resultSet = 0; resultSet < resultSetCount; resultSet++)
                    {
                        while (reader.Read())
                            await mapDataFunctionName(resultSet, oRef, reader);
                        reader.NextResult();
                    }
                }
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
                await sqlCmd.DisposeAsync();
                if (sqlTran != null)
                    await sqlTran.DisposeAsync();
                await CloseConnectionAsync();
            }
        }

        public override async Task<List<T>> ExecuteListAsync<T>(string commandText, CommandType commandType)
        {
            List<T> oLists = new List<T>();
            try
            {
                sqlCmd = new NpgsqlCommand();
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
                sqlCmd = new NpgsqlCommand();
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
            sqlTran = (NpgsqlTransaction)await sqlConn.BeginTransactionAsync();
            sqlCmd = new NpgsqlCommand();
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
            throw new Exception(string.Format("Sql notification is not supporting for postgreSql database."));
        }

        public override void SqlNotificationEnd()
        {
            throw new Exception(string.Format("Sql notification is not supporting for postgreSql database."));
        }

        public override void SqlNotificationDeregisterEvent(SqlNotificationOnSend sqlNotificationOnSend)
        {
            throw new Exception(string.Format("Sql notification is not supporting for postgreSql database."));
        }

        public override List<T> SqlNotification<T>(string commandText, CommandType commandType, SqlNotificationOnSend sqlNotificationOnSend)
        {
            throw new Exception(string.Format("Sql notification is not supporting for postgreSql database."));
        }

        public override List<T> SqlNotification<T>(string table, string columns, SqlNotificationOnSend sqlNotificationOnSend)
        {
            throw new Exception(string.Format("Sql notification is not supporting for postgreSql database."));
        }
        #endregion
    }
}
