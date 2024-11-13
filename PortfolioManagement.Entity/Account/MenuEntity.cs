

using PortfolioManagement.Entity.Master;

namespace PortfolioManagement.Entity.Account
{
    /// <summary>
    /// This class having main entities of table Menu
    /// Created By :: Rekansh Patel
    /// Created On :: 02/03/2018
    /// </summary>
    public class MenuMainEntity
    {
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public MenuMainEntity()
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
    /// This class having entities of table Menu
    /// Created By :: Rekansh Patel
    /// Created On :: 02/03/2018
    /// </summary>
    public class MenuEntity : MenuMainEntity
    {
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public MenuEntity()
        {
            SetDefaulValue();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Get & Set Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get & Set Parent Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Get & Set Page Title
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// Get & Set Icon
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Get & Set Routing
        /// </summary>
        public string Routing { get; set; }

        /// <summary>
        /// Get & Set Order By
        /// </summary>
        public int OrderBy { get; set; }

        /// <summary>
        /// Get & Set Is Menu
        /// </summary>
        public bool IsMenu { get; set; }

        /// <summary>
        /// Get & Set Is Client
        /// </summary>
        public bool IsClient { get; set; }

        /// <summary>
        /// Get & Set Is Public
        /// </summary>
        public bool IsPublic { get; set; }

        public string ParentIdName { get; set; }
        #endregion

        #region Private Methods
        /// <summary>
        /// This function is set properties default value based on its data type in table.
        /// </summary>
        private void SetDefaulValue()
        {
            Description = string.Empty;
            ParentId = 0;
            PageTitle = string.Empty;
            Icon = string.Empty;
            Routing = string.Empty;
            OrderBy = 0;
            IsMenu = false;
            IsClient = false;
            IsPublic = false;
            ParentIdName = string.Empty;
        }
        #endregion
    }

    public class MenuAddEntity
    {
        public List<MenuMainEntity> Menus { get; set; } = new List<MenuMainEntity>();

    }

    public class MenuEditEntity : MenuAddEntity
    {
        public MenuEntity Menu { get; set; } = new MenuEntity();
    }

    public class MenuGridEntity
    {
        public List<MenuEntity> Menus { get; set; } = new List<MenuEntity>();
        public int TotalRecords { get; set; }
    }

    public class MenuParameterEntity : PagingSortingEntity
    {
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public MenuParameterEntity()
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
        /// Get & Set Parent Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Get & Set Role Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Get & Set Language Id
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Get & Set User Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Get & Set IsMobile
        /// </summary>
        public bool IsMobile { get; set; }

        /// <summary>
        /// Get & Set IsClient
        /// </summary>
        public bool IsClient { get; set; }

        #endregion

        #region Private Methods
        /// <summary>
        /// This function is set properties default value based on its data type in table.
        /// </summary>
        private void SetDefaulValue()
        {
            Id = 0;
            Name = string.Empty;
            ParentId = 0;
            LanguageId = 0;
            UserId = 0;
            IsMobile = false;
        }
        #endregion
    }
}