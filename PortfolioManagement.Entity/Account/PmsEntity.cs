using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.Account
{
    public class PmsMainEntity
    {
        public PmsMainEntity()
        {
            SetDefaulValue();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        private void SetDefaulValue()
        {
            Id = 0;
            Name = string.Empty;

        }
    }

    public class PmsEntity : PmsMainEntity
    {
        public PmsEntity()
        {
            SetDefaulValue();
        }
        public bool IsActive { get; set; }
        public string Type { get; set; }
        private void SetDefaulValue()
        {
            IsActive = false;
        }
    }

    public class PmsGridEntity
    {
        public List<PmsEntity> pmss { get; set; } = new List<PmsEntity>();
        public int TotalRecords { get; set; }
    }

    public class PmsParameterEntity : PagingSortingEntity
    {
        public PmsParameterEntity()
        {
            SetDefaulValue();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        private void SetDefaulValue()
        {
            Id = 0;
            Name = string.Empty;

        }
    }
}
