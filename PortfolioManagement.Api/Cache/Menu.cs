using PortfolioManagement.Business.Account;
using PortfolioManagement.Entity.Account;

namespace PortfolioManagement.Api.Cache
{
    public class Menu : Cache
    {
        public static List<MenuEntity> Items
        {
            get
            {
                if (MyCache.Get("Menu") == null)
                {
                    Refresh();
                }
                return MyCache.Get("Menu") as List<MenuEntity>;
            }
            set
            {
                SetCache("Menu", value);
            }
        }

        public static void Refresh()
        {
            MenuBusiness objMenuBusiness = new MenuBusiness(Startup.Configuration);
            SetCache("Menu", objMenuBusiness.SelectList(null));
        }

        public static void Add(MenuEntity objMenuEntity)
        {
            Items.Add(objMenuEntity);
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

        public static void Modify(int Id, MenuEntity objMenuEntity)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Id == Id)
                {
                    Items[i] = objMenuEntity;
                    break;
                }
            }
        }
    }
}