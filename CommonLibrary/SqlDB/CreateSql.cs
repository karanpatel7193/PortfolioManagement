using Microsoft.Extensions.Configuration;

namespace CommonLibrary.SqlDB
{
    /// <summary>
    /// This class use for create MsSql, PgSQL, OleDB, ODBCDB object based on provider.
    /// Created By : Rekansh Patel
    /// Created On : 07/09/2019
    /// </summary>
    public class CreateSql
    {
        #region Constructor
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// Get configurations in constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public CreateSql(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region Private Variable
        private string _connectionStringKey;
        private static string _staticConnectionStringKey;
        #endregion

        #region Properties
        public string ConnectionStringKey
        {
            get { return _connectionStringKey; }
            set { _connectionStringKey = value; }
        }
        public static string CommonConnectionStringKey
        {
            get { return _staticConnectionStringKey; }
            set { _staticConnectionStringKey = value; }
        }
        private string ProviderName
        {
            get
            {
                if (ConnectionStringKey != string.Empty)
                    return MyConvert.ToString(Configuration["ConnectionStrings:" + ConnectionStringKey + ":ProviderName"]).ToLower();
                else if (CommonConnectionStringKey != string.Empty)
                    return MyConvert.ToString(Configuration["ConnectionStrings:" + CommonConnectionStringKey + ":ProviderName"]).ToLower();
                else
                    return string.Empty;
            }
        }
        private string ConnectionString
        {
            get
            {
                if (ConnectionStringKey != string.Empty)
                    return string.Format(MyConvert.ToString(Configuration["ConnectionStrings:" + ConnectionStringKey + ":ConnectionString"]), IPAddress);
                else if (CommonConnectionStringKey != string.Empty)
                    return string.Format(MyConvert.ToString(Configuration["ConnectionStrings:" + CommonConnectionStringKey + ":ConnectionString"]), IPAddress);
                else
                    return string.Empty;
            }
        }
        private string IPAddress
        {
            get;
            set;
        }
        public string Schema
        {
            get;
            set;
        }

        #endregion

        #region Public Methods
        public ISql CreateSqlInstance()
        {
            IPAddress = string.Empty;
            if (ProviderName.ToLower().Contains("system.data.sqlclient"))
                return new MsSql(ConnectionString, Schema);
            else if (ProviderName.ToLower().Contains("oracle.dataaccess.client") || ProviderName.ToLower().Contains("microsoft.ace.oledb"))
                return new OleSql(ConnectionString, Schema);
            else if (ProviderName.ToLower().Contains("npgsql"))
                return new PostgreSql(ConnectionString, Schema);
            else
                return new MsSql(ConnectionString, Schema);
        }

        public ISql CreateSqlInstance(string connectionStringKey, string schema = "")
        {
            IPAddress = string.Empty;
            ConnectionStringKey = connectionStringKey;
            Schema = schema;
            return CreateSqlInstance();
        }

        public ISql CreateSqlInstance(string connectionStringKey, string ipAddress, string schema = "")
        {
            IPAddress = ipAddress;
            ConnectionStringKey = connectionStringKey;
            Schema = schema;
            if (ProviderName.ToLower().Contains("system.data.sqlclient"))
                return new MsSql(ConnectionString, Schema);
            else if (ProviderName.ToLower().Contains("oracle.dataaccess.client") || ProviderName.ToLower().Contains("microsoft.ace.oledb"))
                return new OleSql(ConnectionString, Schema);
            else if (ProviderName.ToLower().Contains("npgsql"))
                return new PostgreSql(ConnectionString, Schema);
            else
                return new MsSql(ConnectionString, Schema);
        }

        #endregion
    }
}
