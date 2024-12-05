using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PortfolioManagement.Api.Common;
using PortfolioManagement.Business.Account;
using PortfolioManagement.Business.Master;
using PortfolioManagement.Business.ScriptView;
using PortfolioManagement.Business.Watchlist;
using PortfolioManagement.Business.Index;
using PortfolioManagement.Business.Master;
using PortfolioManagement.Business.Portfolio;
using PortfolioManagement.Business.Transaction;
using PortfolioManagement.Business.Transaction.StockTransaction;
using PortfolioManagement.Repository.Account;
using PortfolioManagement.Repository.ScriptView;
using PortfolioManagement.Repository.Watchlist;
using PortfolioManagement.Repository.Index;
using PortfolioManagement.Repository.Master;
using PortfolioManagement.Repository.Portfolio;
using PortfolioManagement.Repository.Transaction;
using System.Text;
using PortfolioManagement.Repository.Analysis;
using PortfolioManagement.Business.Analysis;

namespace PortfolioManagement.Api
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Portfolio Api", Version = "v1"  });
            });

            //Add JWT token authentication with all setting.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = Startup.GetValidationParameters();
            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "forbidScheme";
                options.DefaultForbidScheme = "forbidScheme";
                options.AddScheme<AuthenticationSchemeHandle>("forbidScheme", "Handle Forbidden");
            });
            services.AddScoped<IMenuRepository, MenuBusiness>();
            services.AddScoped<IPmsRepository, PmsBusiness>();
            services.AddScoped<IRoleMenuAccessRepository, RoleMenuAccessBusiness>();
            services.AddScoped<IUserReppository, UserBusiness>();
            services.AddScoped<IScriptViewAboutCompanyRpository, ScriptViewAboutCompanyBusiness>();
            services.AddScoped<IScriptViewChartRepository, ScriptViewChartBusiness>();
            services.AddScoped<IScriptViewCorporateActionRepository, ScriptViewCorporateActionBusiness>();
            services.AddScoped<IScriptViewOverviewRepository, ScriptViewOverviewBusiness>();
            services.AddScoped<IScriptViewPeersRepository, ScriptViewPeersBusiness>();
            services.AddScoped<IScriptViewRangeRepository, ScriptViewRangeBusiness>();
            services.AddScoped<IWatchlistRepository, WatchlistBusiness>();
            services.AddScoped<IVolumeRepository, VolumeBusiness>();
            services.AddScoped<IHeaderRepository, HeaderBusiness>();
            services.AddScoped<IIndexChartRepository, IndexChartBusiness>();
            services.AddScoped<IAccounRepository, AccountBusiness>();
            services.AddScoped<IBrokerRepository, BrokerBusiness>();
            services.AddScoped<IMasterRepositoroy, MasterBusiness>();
            services.AddScoped<IScriptRepository, ScriptBusiness>();
            services.AddScoped<ISplitBonusRepository, SplitBonusBusiness>();
            services.AddScoped<IStockTransactionRepository, StockTransactionBusiness>();
            services.AddScoped<IDataProcessorRepository, DataProcessorBusiness>();
            services.AddScoped<IIndexFiiDiiRepository, IndexFiiDiiBusiness>();
            services.AddScoped<IPortfolioRepository, PortfolioBusiness>();
            services.AddScoped<IPortfolioDatewiseReository, PortfolioDatewiseBusiness>();
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), AppSettings.PathDocumentUpload)),
            //    RequestPath = "/" + AppSettings.PathDocumentUpload + "/"
            //});
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"UploadedFiles")),
                RequestPath = "/UploadedFiles"
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(AppSettings.ApplicationSwagger, "Portfolio Api - v1");
            });

            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.Run();
        }

        public static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = AppSettings.SecurityTokenIssuer,
                ValidAudience = AppSettings.SecurityTokenKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.SecurityTokenKey))
            };
        }
    }
}
