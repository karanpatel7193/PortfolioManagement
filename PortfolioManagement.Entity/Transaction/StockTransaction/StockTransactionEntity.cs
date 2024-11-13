using PortfolioManagement.Entity.Account;
using PortfolioManagement.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PortfolioManagement.Entity.Transaction.StockTransaction
{
    public class StockTransactionMainEntity
    {
        #region Public Properties
        /// <summary>
        /// Get & Set Id
        /// </summary>
        public long Id { get; set; } = 0;
        #endregion
    }

    public class StockTransactionEntity : StockTransactionMainEntity
    {
        #region Public Properties
        public DateTime Date { get; set; } = DateTime.MinValue;
        public byte AccountId { get; set; } = 0;
        public string AccountName { get; set; }=string.Empty;
        public int TransactionTypeId {  get; set; } = 0;
        public string TransactionTypeName { get; set; } = string.Empty;
        public short ScriptId {  get; set; } = 0;
        public string ScriptName { get; set; } = string.Empty;
        public short Qty { get; set; } = 0;
        public double Price { get; set; } = 0;   
        public byte BrokerId { get; set; } = 0; 
        public string BrokerName { get; set; } = string.Empty;
        public double Brokerage { get; set; } = 0;   
        public double Buy { get; set; } = 0;
        public double Sell { get; set; } = 0;
        public double Dividend { get; set; } = 0;
        [JsonIgnore]
        public int PmsId { get; set; } = 0; 
        public DateTime FromDate { get; set; } = DateTime.MinValue;
        public DateTime ToDate { get; set; } = DateTime.MinValue;



        #endregion
    }

    public class StockTransactionGridEntity
    {
        public List<StockTransactionEntity> Stocks { get; set; } = new List<StockTransactionEntity>();
        public int TotalRecords { get; set; }
    }
    
    public class StockTransactionParameterEntity : PagingSortingEntity
    {

        #region Public Properties
        /// <summary>
        /// Get & Set Id
        /// </summary>
        public int Id { get; set; } = 0;

        [JsonIgnore]
        public int PmsId { get; set; } = 0;


        public byte AccountId { get; set; } = 0;
        public int TransactionTypeId { get; set; } = 0;
        public short ScriptId { get; set; } = 0;
        public byte BrokerId { get; set; } = 0;
        public DateTime FromDate { get; set; } = DateTime.MinValue;
        public DateTime ToDate { get; set; } = DateTime.MinValue;

        #endregion
    }
    public class StockTransactionListEntity
    {
        public List<AccountMainEntity> Accounts { get; set; } = new List<AccountMainEntity>();
        public List<BrokerEntity> Brokers { get; set; } = new List<BrokerEntity>();
        public List<ScriptMainEntity> Scripts { get; set; } = new List<ScriptMainEntity>();
    }

    public class StockTransactionSummaryEntity
    {
        public byte AccountId { get; set; } = 0;
        public string AccountName { get; set; } = string.Empty;
        public double Buy { get; set; } = 0;
        public double Sell { get; set; } = 0;
        public double Dividend { get; set; } = 0;
    }

    
    
}

