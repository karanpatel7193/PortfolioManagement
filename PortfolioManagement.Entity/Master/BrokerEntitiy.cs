using System.Text.Json.Serialization;

namespace PortfolioManagement.Entity.Master
{
    //BrokerMainEntity
    public class BrokerMainEntity
    {
        public byte Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;

    }

    //BrokerEntity
    public class BrokerEntity : BrokerMainEntity
    {
        public int BrokerTypeId { get; set; } = 0;
        public double BuyBrokerage { get; set; } = 0;  
        public double SellBrokerage { get; set; } = 0;
        public string BrokerType { get; set; } = string.Empty;
        public byte AccountId { get; set; } = 0;

        [JsonIgnore]
        public int PmsId { get; set; } = 0;
    }
    //BrokerGridEntity
    public class BrokerGridEntity
    {
        
        public List<BrokerEntity> Brokers { get; set; } = new List<BrokerEntity>();
        public int TotalRecords { get; set; }
    }
    public class BrokerParameterEntity : PagingSortingEntity
    {
        public byte Id { get; set; } = 0;

        public string Name { get; set; } = string.Empty;

        public int BrokerTypeId { get; set; } = 0;

        [JsonIgnore]
        public int PmsId { get; set; } = 0;
    }

}

