using PortfolioManagement.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.Account
{

    public enum RoleType
    {
        Admin = 1,
        PmsAdmin = 2,
        Master = 3,
        Accountant = 4
    }
    /// <summary>
    /// This class having main entities of table Role
    /// Created By :: Rekansh Patel
    /// Created On :: 02/03/2018
    /// </summary>
    public class RoleMainEntity
    {
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public RoleMainEntity()
        {
            SetDefaulValue();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Get & Set Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get & Set Name
        /// </summary>
        public string Name { get; set; }


        #endregion

        #region Private Methods
        /// <summary>
        /// This function is set properties default value based on its data type in table.
        /// </summary>
        private void SetDefaulValue()
        {
            Id = 0;
            Name = string.Empty;

        }
        #endregion
    }

    /// <summary>
    /// This class having entities of table Role
    /// Created By :: Rekansh Patel
    /// Created On :: 02/03/2018
    /// </summary>
    public class RoleEntity : RoleMainEntity
    {
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public RoleEntity()
        {
            SetDefaulValue();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Get & Set Is Public
        /// </summary>
        public bool IsPublic { get; set; }
        public List<RoleMenuAccessEntity> RoleMenuAccesss { get; set; } = new List<RoleMenuAccessEntity>();

        #endregion

        #region Private Methods
        /// <summary>
        /// This function is set properties default value based on its data type in table.
        /// </summary>
        private void SetDefaulValue()
        {
            IsPublic = false;
        }
        #endregion
    }

    public class RoleGridEntity
    {
        public List<RoleEntity> Roles { get; set; } = new List<RoleEntity>();
        public int TotalRecords { get; set; }
    }

    public class RoleParameterEntity : PagingSortingEntity
    {
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public RoleParameterEntity()
        {
            SetDefaulValue();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Get & Set Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get & Set Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get & Set Is Public
        /// </summary>
        public bool? IsPublic { get; set; }

        #endregion

        #region Private Methods
        /// <summary>
        /// This function is set properties default value based on its data type in table.
        /// </summary>
        private void SetDefaulValue()
        {
            Id = 0;
            Name = string.Empty;

        }
        #endregion
    }
}
