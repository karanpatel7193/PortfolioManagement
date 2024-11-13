using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.ScriptView
{
    public class ScriptViewAboutCompanyEntity
    {
        public string Name { get; set; }
        public decimal BseCode { get; set; }
        public string NseCode { get; set; }
        public string IndustryName { get; set; } = string.Empty;
        public string ISINCode { get; set; }

    }
}
