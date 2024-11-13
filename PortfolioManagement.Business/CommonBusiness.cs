using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;

namespace PortfolioManagement.Business
{
    public class CommonBusiness
    {
        public static IConfiguration configuration;
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public CommonBusiness(IConfiguration config)
        {
            configuration = config;
            this.ConnectionStringKey = "Default";
        }
        public CommonBusiness(IConfiguration config, string connectionStringKey)
        {
            configuration = config;

            this.ConnectionStringKey = connectionStringKey;
        }
        #endregion

        #region public override Properties
        public string ConnectionStringKey { get; set; }
        #endregion

        #region Private Methods
        public ISql CreateSqlInstance()
        {
            CreateSql createSql = new CreateSql(configuration);
            return createSql.CreateSqlInstance(ConnectionStringKey);
        }
        public ISql CreateSqlInstance(string connectionStringKey)
        {
            CreateSql createSql = new CreateSql(configuration);
            return createSql.CreateSqlInstance(connectionStringKey);
        }
        #endregion
    }
}
