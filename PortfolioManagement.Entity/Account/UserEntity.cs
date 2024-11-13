using DocumentFormat.OpenXml.Office2016.Presentation.Command;
using PortfolioManagement.Entity.Account;
using PortfolioManagement.Entity.Common;
using System.Text.Json.Serialization;

namespace PortfolioManagement.Entity.Master
{
    /// <summary>
    /// This class having main entities of table User
    /// Created By :: Rekansh Patel
    /// Created On :: 02/06/2018
    /// </summary>
    public class UserMainEntity
    {
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public UserMainEntity()
        {
            SetDefaulValue();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Get & Set Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Get & Set First Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Get & Set Last Name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Get & Set Email
        /// </summary>
        public string Email { get; set; }
        #endregion

        #region Private Methods
        /// <summary>
        /// This function is set properties default value based on its data type in table.
        /// </summary>
        private void SetDefaulValue()
        {
            Id = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
        }
        #endregion
    }

    /// <summary>
    /// This class having entities of table User
    /// Created By :: Rekansh Patel
    /// Created On :: 02/06/2018
    /// </summary>
    public class UserEntity : UserMainEntity
    {
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public UserEntity()
        {
            SetDefaulValue();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Get & Set Middle Name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Get & Set Role Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Get & Set Gender
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Get & Set Birth Date
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Get & Set Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Get & Set Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Get & Set Password Salt
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Get & Set Old Password
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// Get & Set Phone Number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Get & Set Last Update Date Time
        /// </summary>
        public DateTime LastUpdateDateTime { get; set; }

        /// <summary>
        /// Get & Set Image Src
        /// </summary>
        public string ImageSrc { get; set; }

        /// <summary>
        /// Get & Set Role Name
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Get & Set Is Active
        /// </summary>
        public bool IsActive { get; set; }

        [JsonIgnore]
        public int PmsId { get; set; }

        public string Type { get; set; } 

        public string PmsName { get; set; }

        #endregion

        #region Private Methods
        /// <summary>
        /// This function is set properties default value based on its data type in table.
        /// </summary>
        private void SetDefaulValue()
        {
            MiddleName = string.Empty;
            RoleId = 0;
            Gender = 0;
            BirthDate = DateTime.MinValue;
            Username = string.Empty;
            Password = string.Empty;
            PasswordSalt = string.Empty;
            OldPassword = string.Empty;
            PhoneNumber = string.Empty;
            LastUpdateDateTime = DateTime.MinValue;
            ImageSrc = string.Empty;
            RoleName = string.Empty;
            IsActive = false;
            PmsId = 0;
            Type = string.Empty;
            PmsName = string.Empty;
        }
        #endregion
    }

    public class UserAddEntity
    {
        public List<RoleMainEntity> Roles { get; set; } = new List<RoleMainEntity>();
    }

    public class UserEditEntity : UserAddEntity
    {
        public UserEntity User { get; set; } = new UserEntity();
    }

    public class UserGridEntity
    {
        public List<UserEntity> Users { get; set; } = new List<UserEntity>();
        public int TotalRecords { get; set; }
    }

    public class UserListEntity : UserGridEntity
    {
        public List<RoleMainEntity> Roles { get; set; } = new List<RoleMainEntity>();
    }

    public class UserParameterEntity : PagingSortingEntity
    {
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public UserParameterEntity()
        {
            SetDefaulValue();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Get & Set Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Get & Set First Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Get & Set Last Name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Get & Set Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Get & Set Role Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Get & Set Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Get & Set Phone Number
        /// </summary>
        public string PhoneNumber { get; set; }

        public string API_URL { get; set; }

        [JsonIgnore]
        public int PmsId { get; set; } 
        #endregion

        #region Private Methods
        /// <summary>
        /// This function is set properties default value based on its data type in table.
        /// </summary>
        private void SetDefaulValue()
        {
            Id = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            RoleId = 0;
            Username = string.Empty;
            PhoneNumber = string.Empty;
            API_URL = string.Empty;
            PmsId = 0;
        }
        #endregion
    }

    /// <summary>
    /// This class having entities of table User
    /// Created By :: Rekansh Patel
    /// Created On :: 04/23/2017
    /// </summary>
    public class UserLoginEntity : UserMainEntity
    {
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public UserLoginEntity()
        {
            SetDefaulValue();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Get & Set User Login Log Id
        /// </summary>
        public long UserLoginLogId { get; set; }

        /// <summary>
        /// Get & Set Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Get & Set User RoleId
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Get & Set User RoleName
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Get & Set Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Get & Set Image Src
        /// </summary>
        public string ImageSrc { get; set; }

        [JsonIgnore]
        public int PmsId { get; set; } = 0;

        public string PmsName { get; set; } 

        private List<RoleMenuAccessEntity> roleMenuAccessEntitys;
        public List<RoleMenuAccessEntity> RoleMenuAccesss
        {
            get
            {
                if (roleMenuAccessEntitys == null)
                    roleMenuAccessEntitys = new List<RoleMenuAccessEntity>();
                return roleMenuAccessEntitys;
            }
            set
            {
                roleMenuAccessEntitys = value;
            }
        }

        private List<MenuEntity> menuEntitys;
        public List<MenuEntity> Menus
        {
            get
            {
                if (menuEntitys == null)
                    menuEntitys = new List<MenuEntity>();
                return menuEntitys;
            }
            set
            {
                menuEntitys = value;
            }
        }
        public List<MasterValueEntity> MasterValues { get; set; } = new List<MasterValueEntity>();
        #endregion

        #region Private Methods
        /// <summary>
        /// This function is set properties default value based on its data type in table.
        /// </summary>
        private void SetDefaulValue()
        {
            UserLoginLogId = 0;
            Username = string.Empty;
            RoleId = 0;
            RoleName = string.Empty;
            Token = string.Empty;
            ImageSrc = string.Empty;
            PmsName = string.Empty;
        }

        #endregion
    }

    public class UserImageEntity : FileEntity
    {
        public long UserId { get; set; }
        public string ImageSrc { get; set; }
    }

    public class UserImageResultEntity
    {
        public string Status { get; set; }
        public string ImageSrc { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class UsersEntity
    {
        public int id { get; set; } = 0;
        public string username { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public int gender { get; set; } = 0;

    }
}
