using PortfolioManagement.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.ScriptView
{
    public class ScriptViewPriceEntity
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public double Price { get; set; } = 0;
        public long Volume { get; set; } = 0;
        public double PreviousDay { get; set; } = 0;
        public double Open { get; set; } = 0;
        public double Close { get; set; } = 0;
        public double High { get; set; } = 0;
        public double Low { get; set; } = 0;
       
    }

    public class ScriptViewChartEntity
    {
        public ScriptMainEntity Script { get; set; } = new ScriptMainEntity();
        public List<ScriptViewPriceEntity> Prices { get; set; } = new List<ScriptViewPriceEntity>();

        public List<string> Dates { get; set; } = new List<string>();
        public List<object[]> CandelSeriesData { get; set; } = new List<object[]>();
        public List<double> PriceSeriesData { get; set; } = new List<double>();
        public List<double> VolumeSeriesData { get; set; } = new List<double>();
    }

   




}

