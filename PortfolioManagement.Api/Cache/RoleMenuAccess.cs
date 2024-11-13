using PortfolioManagement.Business.Account;
using PortfolioManagement.Entity.Account;

namespace PortfolioManagement.Api.Cache
{
    public class RoleMenuAccess : Cache
    {
        public static List<RoleMenuAccessEntity> Items
        {
            get
            {
                if (MyCache.Get("RoleMenuAccess") == null)
                {
                    Refresh();
                }
                return MyCache.Get("RoleMenuAccess") as List<RoleMenuAccessEntity>;
            }
            set
            {
                SetCache("RoleMenuAccess", value);
            }
        }

        public static void Refresh()
        {
            RoleMenuAccessBusiness objRoleMenuAccessBusiness = new RoleMenuAccessBusiness(Startup.Configuration);
            SetCache("RoleMenuAccess", objRoleMenuAccessBusiness.SelectList());
        }

        public static void Add(RoleMenuAccessEntity objRoleMenuAccessEntity)
        {
            Items.Add(objRoleMenuAccessEntity);
        }

        public static void Remove(int Id)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Id == Id)
                {
                    Items.RemoveAt(i);
                    break;
                }
            }
        }

        public static void Modify(int Id, RoleMenuAccessEntity objRoleMenuAccessEntity)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Id == Id)
                {
                    Items[i] = objRoleMenuAccessEntity;
                    break;
                }
            }
        }
    }
}