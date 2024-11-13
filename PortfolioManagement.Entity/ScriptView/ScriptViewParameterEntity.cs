using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.ScriptView
{
    public class ScriptViewParameterEntity
    {
        public int ScriptId { get; set; } = 0;   
        public DateTime FromDate { get; set; } = DateTime.MinValue;
        public DateTime ToDate { get; set; }= DateTime.MinValue;
    }
}
