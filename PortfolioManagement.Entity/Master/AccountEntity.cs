using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PortfolioManagement.Entity.Master
{
    public class AccountMainEntity
    {
        public byte Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
    }

    public class AccountEntity : AccountMainEntity
    {
        public List<AccountBrokerSelectEntity> Brokers { get; set; } = new List<AccountBrokerSelectEntity>();

        [JsonIgnore]
        public int PmsId { get; set; } = 0;


    }

    public class AccountGridEntity
    {
        public List<AccountEntity> Accounts { get; set; } = new List<AccountEntity>();
        public int TotalRecords { get; set; }
    }

    public class AccountParameterEntity : PagingSortingEntity
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public int PmsId { get; set; } = 0;

    }

    public class AccountAddEntity
    {
        public List<AccountBrokerSelectEntity> Brockers { get; set; } = new List<AccountBrokerSelectEntity>();
    }
    public class AccountEditEntity
    {
        public AccountEntity Account { get; set; } = new AccountEntity();
    }
    public class AccountBrokerSelectEntity : BrokerMainEntity
    {
        public bool IsSelected { get; set; } = false; 
    }
}



