using PortfolioManagement.Business.Master;
using PortfolioManagement.Entity.Master;

namespace PortfolioManagement.Api.Cache
{
    public class MasterValue : Cache
    {
        public static List<MasterValueEntity> Items
        {
            get
            {
                if (MyCache.Get("MasterValue") == null)
                {
                    Refresh();
                }
                return MyCache.Get("MasterValue") as List<MasterValueEntity>;
            }
            set
            {
                SetCache("MasterValue", value);
            }
        }

        public static void Refresh()
        {
            MasterValueBusiness objMasterValueBusiness = new MasterValueBusiness(Startup.Configuration);
            SetCache("MasterValue", objMasterValueBusiness.SelectForCache());
        }
    }
}