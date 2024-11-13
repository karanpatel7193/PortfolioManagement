using System.Data;

namespace CommonLibrary.SqlDB
{
    public class SqlParameter
    {
        #region Private variables
        private string parameterName;

        private DbType dbType;

        private ParameterDirection direction;

        private object parameterValue;

        #endregion

        #region Public Properties
        public string ParameterName
        {
            get { return parameterName; }
            set { parameterName = value; }
        }

        public DbType DBType
        {
            get { return dbType; }
            set { dbType = value; }
        }

        public ParameterDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public object ParameterValue
        {
            get { return parameterValue; }
            set { parameterValue = value; }
        }
        #endregion

        #region Constructors
        public SqlParameter()
        {
        }

        public SqlParameter(string _ParameterName, object _ParameterValue)
        {
            ParameterName = _ParameterName;
            ParameterValue = _ParameterValue;
            DBType = DbType.String;
            Direction = ParameterDirection.Input;
        }

        public SqlParameter(string _ParameterName, DbType _DBType, ParameterDirection _Direction, object _ParameterValue)
        {
            ParameterName = _ParameterName;
            DBType = _DBType;
            Direction = _Direction;
            ParameterValue = _ParameterValue;
        }
        #endregion
    }
}
