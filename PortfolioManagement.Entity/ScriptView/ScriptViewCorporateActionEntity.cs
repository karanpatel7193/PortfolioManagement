using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.ScriptView
{
    public class ScriptViewCorporateActionBonusEntity
    {
        public int FromRatio { get; set; }
        public int ToRatio { get; set; }
        public DateTime AnnounceDate { get; set; }
        public DateTime RewardDate { get; set; }
    }
    public class ScriptViewCorporateActionSplitEntity
    {
        public double OldFaceValue { get; set; }
        public double NewFaceValue { get; set; }
        public DateTime AnnounceDate { get; set; }
        public DateTime RewardDate { get; set; }
        public bool IsSplit { get; set; }
    }
}
