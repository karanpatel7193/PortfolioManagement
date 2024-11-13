using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.Account
{
    /// <summary>   
    /// This class having entities of table RoleMenuAccess   
    /// Created By :: Rekansh Patel   
    /// Created On :: 05/27/2017   
    /// </summary>
    public class RoleMenuAccessEntity
    {
        #region Constructor
        /// <summary>   
        /// This construction is set properties default value based on its data type in table.   
        /// </summary>
        public RoleMenuAccessEntity()
        {
            SetDefaulValue();
        }

        #endregion

        #region Public Properties
        /// <summary> 
        /// Get & Set Id 
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary> 
        /// Get & Set Role Id 
        /// </summary>
        public int RoleId
        {
            get;
            set;
        }

        /// <summary> 
        /// Get & Set Menu Id 
        /// </summary>
        public int MenuId
        {
            get;
            set;
        }

        /// <summary> 
        /// Get & Set Can Insert 
        /// </summary>
        public bool CanInsert
        {
            get;
            set;
        }

        /// <summary> 
        /// Get & Set Can Update 
        /// </summary>
        public bool CanUpdate
        {
            get;
            set;
        }

        /// <summary> 
        /// Get & Set Can Delete 
        /// </summary>
        public bool CanDelete
        {
            get;
            set;
        }

        /// <summary> 
        /// Get & Set Can View 
        /// </summary>
        public bool CanView
        {
            get;
            set;
        }
        public string MenuIdName { get; set; }

        #endregion

        #region Private Methods
        /// <summary>   
        /// This function is set properties default value based on its data type in table.   
        /// </summary>
        private void SetDefaulValue()
        {
            Id = 0;
            RoleId = 0;
            MenuId = 0;
            CanInsert = false;
            CanUpdate = false;
            CanDelete = false;
            CanView = false;
            MenuIdName = string.Empty;
        }
        #endregion
    }

}

