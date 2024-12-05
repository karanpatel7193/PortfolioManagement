using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.Transaction
{
    public interface IDataProcessorRepository
    {
        public  Task StartScrap(DateTime current, DateTime fromTime, DateTime toTime, DateTime fiiDiiTime, string nifty500_URL, string nifty500_ApiURL, string nseScript_URL, string nifty50_URL, string sensex_URL, string fiiDii_URL, string nseScript_ApiURL, string nifty50_ApiURL, string sensex_ApiURL, string fiiDii_ApiURL);

    }
}
