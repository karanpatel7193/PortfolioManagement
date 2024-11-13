using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.Master
{
    public class MasterEntity
    {
        public short Id { get; set; } = 0;
        public string Type { get; set; } = string.Empty;
        public List<MasterValueEntity> MasterValues { get; set; } = new List<MasterValueEntity>();

    }

    public class MasterParameterEntity
    {
        public short Id { get; set; }
    }

}
