using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.Analysis
{
    public class VolumeEntity
    {
        public double ScriptId { get; set; } = 0;
        public string ScriptName { get; set; } = string.Empty;
        public double Volume { get; set; } = 0;
        public double WeekAverageVolume { get; set; } = 0;
        public double MonthAverageVolume { get; set; } = 0;
        public double WeekPercentage { get; set; } = 0;
        public double MonthPercentage { get; set; } = 0;
        public int NewsCount { get; set; } = 0; //integer 
        
    }
    public class VolumeGridEntity
    {
        public List<VolumeEntity> Volumes { get; set; } = new List<VolumeEntity>();
    }

    public class VolumeParameterEntity
    {
        public DateTime DateTime { get; set; } = new DateTime(0);
    }
}
