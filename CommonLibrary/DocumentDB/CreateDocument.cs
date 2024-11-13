using Microsoft.Extensions.Configuration;

namespace CommonLibrary.DocumentDB
{
    public class CreateDocument<TEntity> where TEntity : IBaseEntity
    {
        #region Constructor
        public IConfiguration Configuration { get; private set; }

        public string CollectionName { get; set; }

        /// <summary>
        /// Get configurations in constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public CreateDocument(IConfiguration configuration, string collectionName)
        {
            Configuration = configuration;
            CollectionName = collectionName;
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
                    return MyConvert.ToString(Configuration["ConnectionStrings:" + ConnectionStringKey + ":ConnectionString"]);
                else if (CommonConnectionStringKey != string.Empty)
                    return MyConvert.ToString(Configuration["ConnectionStrings:" + CommonConnectionStringKey + ":ConnectionString"]);
                else
                    return string.Empty;
            }
        }
        private string DatabaseName
        {
            get
            {
                if (ConnectionStringKey != string.Empty)
                    return MyConvert.ToString(Configuration["ConnectionStrings:" + ConnectionStringKey + ":DatabaseName"]);
                else if (CommonConnectionStringKey != string.Empty)
                    return MyConvert.ToString(Configuration["ConnectionStrings:" + CommonConnectionStringKey + ":DatabaseName"]);
                else
                    return string.Empty;
            }
        }

        #endregion

        #region Public Methods
        public IDocument<TEntity> CreateDocumentInstance()
        {
            if (ProviderName.ToLower().Contains("mongodb.driver"))
                return new Mongo<TEntity>(ConnectionString, DatabaseName, CollectionName);
            else
                return new Mongo<TEntity>(ConnectionString, DatabaseName, CollectionName);
        }

        public IDocument<TEntity> CreateDocumentInstance(string connectionStringKey)
        {
            ConnectionStringKey = connectionStringKey;
            return CreateDocumentInstance();
        }
        #endregion
    }
}
