using PortfolioManagement.Business.Account;
using PortfolioManagement.Entity.Account;

namespace PortfolioManagement.Api.Cache
{
    public class Role : Cache
    {
        public static List<RoleEntity> Items
        {
            get
            {
                if (MyCache.Get("Role") == null)
                {
                    Refresh();
                }
                return MyCache.Get("Role") as List<RoleEntity>;
            }
            set
            {
                SetCache("Role", value);
            }
        }

        public static void Refresh()
        {
            RoleBusiness objRoleBusiness = new RoleBusiness(Startup.Configuration);
            SetCache("Role", objRoleBusiness.SelectList());
        }

        public static void Add(RoleEntity objRoleEntity)
        {
            Items.Add(objRoleEntity);
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

        public static void Modify(int Id, RoleEntity objRoleEntity)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Id == Id)
                {
                    Items[i] = objRoleEntity;
                    break;
                }
            }
        }
    }
}