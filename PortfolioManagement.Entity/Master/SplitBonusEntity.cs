using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.Master
{
    public class SplitBonusMainEntity
    {
        public SplitBonusMainEntity()
        {
            SetDefaulValue();
        }
        public int Id { get; set; }
        private void SetDefaulValue()
        {
            Id = 0;

        }
    }

    public class SplitBonusEntity : SplitBonusMainEntity
    {
        public SplitBonusEntity()
        {
            SetDefaulValue();
        }
        public string NseCode { get; set; } = string.Empty;
        public bool IsSplit { get; set; }
        public bool IsBonus { get; set; }
        public double OldFaceValue { get; set; }
        public double NewFaceValue { get; set; }
        public int FromRatio { get; set; }
        public int ToRatio { get; set; }
        public DateTime AnnounceDate { get; set; }
        public DateTime RewardDate { get; set; }
        public bool IsApply { get; set; }

        private void SetDefaulValue()
        {
            IsSplit = false;
            IsBonus = false;
            OldFaceValue = 0.0;
            NewFaceValue = 0.0;
            FromRatio = 0;
            ToRatio = 0;
            AnnounceDate = DateTime.MinValue;
            RewardDate = DateTime.MinValue;
            IsApply = false;
        }
    }

    public class SplitBonusGridEntity
    {
        public List<SplitBonusEntity> SplitBonuss { get; set; } = new List<SplitBonusEntity>();
        public int TotalRecords { get; set; }
    }

    public class SplitBonusParameterEntity
    {
        public SplitBonusParameterEntity()
        {
            SetDefaulValue();
        }
        public int Id { get; set; }
        public int ScriptID { get; set; }
        private void SetDefaulValue()
        {
            Id = 0;
            ScriptID = 0;

        }
    }
}
