using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Master;
using PortfolioManagement.Entity.Account;
using PortfolioManagement.Entity.Master;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PortfolioManagement.Api.Controllers.Master
{
    /// <summary>
    /// This API is used for user-related operations like listing, inserting, updating, and deleting users from the database.
    /// </summary>
    [Route("account/user")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UserController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region Interface public methods
        /// <summary>
        /// Get all columns information for perticular user record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getRecord/{Id:long}", Name = "account.user.record")]
        [AuthorizeAPI(pageName: "User", pageAccess: PageAccessValues.View)]

        public async Task<Response> GetForRecord(long Id)
        {
            Response response;
            try
            {
                UserBusiness userBusiness = new UserBusiness(Startup.Configuration);
                response = new Response(await userBusiness.SelectForRecord(Id));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get main columns informations for bind user LOV
        /// </summary>
        /// <param name="userParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getLovValue", Name = "account.user.lovValue")]
        [AuthorizeAPI(pageName: "User", pageAccess: PageAccessValues.IgnoreAuthorization)]

        public async Task<Response> GetForLOV(UserParameterEntity userParameterEntity)
        {
            Response response;
            try
            {
                UserBusiness userBusiness = new UserBusiness(Startup.Configuration);
                response = new Response(await userBusiness.SelectForLOV(userParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get user's page all LOV data when user page open in add mode.
        /// </summary>
        /// <param name="userParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getAddMode", Name = "account.user.addMode")]
        [AuthorizeAPI(pageName: "User", pageAccess: PageAccessValues.Insert)]

        public async Task<Response> GetForAdd(UserParameterEntity userParameterEntity)
        {
            Response response;
            try
            {
                UserBusiness userBusiness = new UserBusiness(Startup.Configuration);
                response = new Response(await userBusiness.SelectForAdd(userParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get user's page all LOV data and all columns information for edit record when user page open in edit mode.
        /// </summary>
        /// <param name="userParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getEditMode", Name = "account.user.editMode")]
        [AuthorizeAPI(pageName: "User", pageAccess: PageAccessValues.Update)]

        public async Task<Response> GetForEdit(UserParameterEntity userParameterEntity)
        {
            Response response;
            try
            {
                UserBusiness userBusiness = new UserBusiness(Startup.Configuration);
                response = new Response(await userBusiness.SelectForEdit(userParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get user list for bind grid.
        /// </summary>
        /// <param name="userParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getGridData", Name = "account.user.gridData")]
        [AuthorizeAPI(pageName: "User", pageAccess: PageAccessValues.View)]

        public async Task<Response> GetForGrid(UserParameterEntity userParameterEntity)
        {
            Response response;
            try
            {
                userParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                UserBusiness userBusiness = new UserBusiness(Startup.Configuration);
                response = new Response(await userBusiness.SelectForGrid(userParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get user's page all LOV data and grid result when user page open in list mode.
        /// </summary>
        /// <param name="userParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getListMode", Name = "account.user.listMode")]
        [AuthorizeAPI(pageName: "User", pageAccess: PageAccessValues.View)]

        public async Task<Response> GetForList(UserParameterEntity userParameterEntity)
        {
            Response response;
            try
            {
                userParameterEntity.PmsId = AuthenticateCliam.PmsId(Request);
                UserBusiness userBusiness = new UserBusiness(Startup.Configuration);
                response = new Response(await userBusiness.SelectForList(userParameterEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Insert record in user table.
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("insert", Name = "account.user.insert")]
        [AuthorizeAPI(pageName: "User", pageAccess: PageAccessValues.Insert)]

        public async Task<Response> Insert(UserEntity userEntity)
        {
            Response response;
            try
            {
                if (userEntity.RoleId == 1)
                    throw new Exception("Invalid role");
                userEntity.PmsId = AuthenticateCliam.PmsId(Request);
                UserBusiness userBusiness = new UserBusiness(Startup.Configuration);
                response = new Response(await userBusiness.Insert(userEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Insert record in user table.
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("registration", Name = "account.user.registration")]
        //[AuthorizeAPI(pageName: "User", pageAccess: PageAccessValues.Insert)]

        public async Task<Response> Registration(UserEntity userEntity)
        {
            Response response;
            try
            {
                UserBusiness userBusiness = new UserBusiness(Startup.Configuration);
                userEntity.RoleId = (int)RoleType.PmsAdmin;
                response = new Response(await userBusiness.Registration(userEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Update record in user table.
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update", Name = "account.user.update")]
        [AuthorizeAPI(pageName: "User", pageAccess: PageAccessValues.Update)]

        public async Task<Response> Update(UserEntity userEntity)
        {
            Response response;
            try
            {
                UserBusiness userBusiness = new UserBusiness(Startup.Configuration);
                response = new Response(await userBusiness.Update(userEntity));
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Delete record from user table.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete/{Id:long}", Name = "account.user.delete")]
        [AuthorizeAPI(pageName: "User", pageAccess: PageAccessValues.Delete)]

        public async Task<Response> Delete(long Id)
        {
            Response response;
            try
            {
                UserBusiness userBusiness = new UserBusiness(Startup.Configuration);
                await userBusiness.Delete(Id);
                response = new Response();
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
        #endregion

        #region Other public methods
        /// <summary>
        /// Validate username and password when login
        /// If user validate then return user main information, menu and role access.
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("validateLogin", Name = "account.user.validateLogin")]
        [AuthorizeAPI(pageName: "User", pageAccess: PageAccessValues.IgnoreAuthentication)]

        public async Task<Response> ValidateLogin(UserEntity userEntity)
        {
            Response response;
            try
            {
                UserBusiness userBusiness = new UserBusiness(Startup.Configuration);
                UserLoginEntity userLoginEntity = await userBusiness.ValidateLogin(userEntity);
                userLoginEntity.ImageSrc = userLoginEntity.ImageSrc.Replace("##API_URL##", AppSettings.API_URL);
                userLoginEntity.Token = GenerateToken(userLoginEntity.Username, userLoginEntity.Id, userLoginEntity.RoleId, userLoginEntity.PmsId);
                if (userLoginEntity != null && userLoginEntity.Id != 0)
                {
                    if (!Cache.Role.Items.Where(r => r.Id == userLoginEntity.RoleId).FirstOrDefault().IsPublic)
                    {
                        userLoginEntity.RoleMenuAccesss = Cache.RoleMenuAccess.Items.Where(rma => rma.RoleId == userLoginEntity.RoleId).ToList();
                        var menus = from m in Cache.Menu.Items
                                    join rm in userLoginEntity.RoleMenuAccesss on m.Id equals rm.MenuId
                                    select m;
                        userLoginEntity.Menus = menus.ToList(); 
                        userLoginEntity.Menus.Union(Cache.Menu.Items.Where(m => m.IsPublic));
                    }

                    userLoginEntity.MasterValues = Cache.MasterValue.Items;
                }
                response = new Response(userLoginEntity);
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        [HttpPost]
        [Route("resetPassword", Name = "account.user.resetPassword")]
        [AuthorizeAPI(pageName: "User", pageAccess: PageAccessValues.IgnoreAuthentication)]

        public async Task<Response> ResetPassword(UserEntity userEntity)
        {
            Response response;
            try
            {
                UserBusiness userBusiness = new UserBusiness(Startup.Configuration);
                userEntity = await userBusiness.ResetPassword(userEntity.Username);

                if (userEntity.Id > 0)
                {
                    //if (userEntity.Email != string.Empty)
                    //    Emails.ResetPassword(userEntity, _hostingEnvironment);
                }
                response = new Response(userEntity);
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        [HttpPost]
        [Route("updatePassword", Name = "account.user.updatePassword")]
        [AuthorizeAPI(pageName: "User", pageAccess: PageAccessValues.IgnoreAuthentication)]

        public async Task<Response> UpdatePassord(UserEntity userEntity)
        {
            Response response;
            try
            {
                string[] ActivationString = userEntity.Username.ToDeHex().Split('#');
                DateTime SentDateTime = new DateTime(MyConvert.ToInt(ActivationString[2].Substring(0, 4)), MyConvert.ToInt(ActivationString[2].Substring(4, 2)), MyConvert.ToInt(ActivationString[2].Substring(6, 2)), MyConvert.ToInt(ActivationString[2].Substring(8, 2)), MyConvert.ToInt(ActivationString[2].Substring(10, 2)), MyConvert.ToInt(ActivationString[2].Substring(12, 2)));
                if (DateTime.UtcNow > SentDateTime.AddHours(AppSettings.LinkExpireDuration))
                    return new Response("expired");

                userEntity.Id = MyConvert.ToInt(ActivationString[0]);
                userEntity.Email = ActivationString[1];
                userEntity.LastUpdateDateTime = DateTime.UtcNow;

                UserBusiness userBusiness = new UserBusiness(Startup.Configuration);
                bool IsUpdate =  await userBusiness.UpdatePassword(userEntity);
                if (IsUpdate)
                    response = new Response("success");
                else
                    response = new Response("fail");
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get user list.
        /// </summary>
        /// <param name="userParameterEntity"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getUserList", Name = "account.user.getUserList")]
        [AuthorizeAPI(pageName: "User", pageAccess: PageAccessValues.View)]

        public async Task<Response> GetUserList()
        {
            Response response;
            try
            {
                UserBusiness userBusiness = new UserBusiness(Startup.Configuration);
                response = new Response(await userBusiness.SelectForUsersTest());
            }
            catch (Exception ex)
            {
                response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
        #endregion
        #region Private methods
        /// <summary>
        /// This function use for generate JWT token.
        /// </summary>
        /// <returns></returns>
        private string GenerateToken(string userName, long userId, int roleId, int pmsId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, roleId.ToString()),
                    new Claim("User", userId.ToString()),
                    new Claim("Pms",  pmsId.ToString())
                }),
                Issuer = AppSettings.SecurityTokenIssuer,
                Audience = AppSettings.SecurityTokenKey,
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppSettings.SecurityTokenKey)), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}
