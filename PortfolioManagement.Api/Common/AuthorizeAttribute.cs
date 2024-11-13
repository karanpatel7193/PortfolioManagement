using CommonLibrary;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using PortfolioManagement.Entity.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PortfolioManagement.Api.Common
{
    public enum PageAccessValues
    {
        View = 0,
        Insert = 1,
        Update = 2,
        Delete = 3,
        IgnoreAuthentication = 10,
        IgnoreAuthorization = 11,
    }

    public class AuthorizeAPIAttribute : TypeFilterAttribute
    {
        public AuthorizeAPIAttribute(string pageName, PageAccessValues pageAccess) : base(typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { pageName, pageAccess };
        }
    }

    public class AuthorizeActionFilter : IAuthorizationFilter
    {
        public string PageName = string.Empty;
        public PageAccessValues PageAccess = PageAccessValues.IgnoreAuthentication;
        
        public AuthorizeActionFilter(string pageName, PageAccessValues pageAccess)
        {
            PageName = pageName;
            PageAccess = pageAccess;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!AppSettings.SecurityTokenEnabled || PageAccess == PageAccessValues.IgnoreAuthentication)
                return;  

            var hasToken = context.HttpContext.Request.Headers.Any(x => x.Key?.ToLower() == "oauth");
            if (hasToken)
            {
                string encryptedToken = context.HttpContext.Request.Headers.FirstOrDefault(x => x.Key?.ToLower() == "oauth").Value.FirstOrDefault().Replace("Bearer ", string.Empty);
                var jwtHandler = new JwtSecurityTokenHandler();
                //JwtSecurityToken jwtToken = jwtHandler.ReadToken(encryptedToken) as JwtSecurityToken;

                SecurityToken validatedToken;
                try
                {
                    ClaimsPrincipal principal = jwtHandler.ValidateToken(encryptedToken, Startup.GetValidationParameters(), out validatedToken);

                    if(!principal.Identity.IsAuthenticated)
                        context.Result = new ForbidResult();
                    else
                    {
                        if (PageAccess == PageAccessValues.IgnoreAuthorization)
                            return;

                        int menuId = (from m in Cache.Menu.Items
                                      where m.Name == PageName
                                      select m.Id).FirstOrDefault();

                        int roleId = (from r in Cache.Role.Items
                                      where r.Id.ToString() == principal.Claims.Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").FirstOrDefault().Value
                                      select r.Id).FirstOrDefault();


                        RoleMenuAccessEntity roleMenuAccessEntity = (from rm in Cache.RoleMenuAccess.Items
                                                                     where rm.MenuId == menuId && rm.RoleId == roleId
                                                                     select rm).FirstOrDefault();

                        if (roleMenuAccessEntity != null && roleMenuAccessEntity.Id > 0)
                        {
                            if (PageAccess == PageAccessValues.Insert && !roleMenuAccessEntity.CanInsert)
                                context.Result = new ForbidResult();
                            if (PageAccess == PageAccessValues.Update && !roleMenuAccessEntity.CanUpdate)
                                context.Result = new ForbidResult();
                            if (PageAccess == PageAccessValues.Delete && !roleMenuAccessEntity.CanDelete)
                                context.Result = new ForbidResult();
                            if (PageAccess == PageAccessValues.View && !roleMenuAccessEntity.CanView)
                                context.Result = new ForbidResult();
                        }
                        else
                            context.Result = new ForbidResult();
                    }

                }
                catch(Exception ex)
                {
                    context.Result = new ForbidResult();
                    ex.WriteLogFile();
                }
            }
            else
                context.Result = new ForbidResult();
        }
    }

    public class AuthenticationSchemeHandle : IAuthenticationHandler
    {
        private HttpContext _context;

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            properties = properties ?? new AuthenticationProperties();
            _context.Response.StatusCode = 401;
            _context.Response.WriteAsync("Access denied.");
            return Task.CompletedTask;
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            _context = context;
            return Task.CompletedTask;
        }
    }

    public class AuthenticateCliam
    {
        private static ClaimsPrincipal getPriciple(HttpRequest request)
        {
            string encryptedToken = request.HttpContext.Request.Headers.FirstOrDefault(x => x.Key?.ToLower() == "oauth").Value.FirstOrDefault().Replace("Bearer ", string.Empty);
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            return jwtHandler.ValidateToken(encryptedToken, Startup.GetValidationParameters(), out validatedToken);
        }

        public static int RoleId(HttpRequest request)
        {
            return MyConvert.ToInt(getPriciple(request).Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value);
        }

        public static int PmsId(HttpRequest request)
        {
            return MyConvert.ToInt(getPriciple(request).Claims.Where(x => x.Type == "Pms").FirstOrDefault().Value);
        }

        public static long UserId(HttpRequest request)
        {
            return MyConvert.ToLong(getPriciple(request).Claims.Where(x => x.Type == "User").FirstOrDefault().Value);
        }
    }
}
