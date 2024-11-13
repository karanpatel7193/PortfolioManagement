using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.Master
{
    // AccountBrokerMainEntity
    public class AccountBrokerMainEntity
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;


    }

    // AccountBrokerEntity
    public class AccountBrokerEntity : AccountBrokerMainEntity
    {
        public byte Id { get; set; } = 0;
        public byte BrokerId { get; set; } = 0;
        public byte AccountId { get; set; } = 0;
        public bool IsSelected { get; set; } = false;
    }

    // AccountBrokerGridEntity
    public class AccountBrokerGridEntity
    {
        public List<AccountBrokerEntity> AccountBrokers { get; set; } = new List<AccountBrokerEntity>();
        public int TotalRecords { get; set; }
    }

    // AccountBrokerParameterEntity
    public class AccountBrokerParameterEntity : PagingSortingEntity
    {
        public int Id { get; set; } = 0;
        public byte BrokerId { get; set; } = 0;
        public byte AccountId { get; set; } = 0;
    }
}
