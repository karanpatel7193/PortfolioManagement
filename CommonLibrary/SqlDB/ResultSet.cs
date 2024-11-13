using System.Collections.Generic;

namespace CommonLibrary.SqlDB
{
    public class ResultSet
    {
        public int ResultIndex { get; set; } = 0;
        public List<dynamic> ResultData = new List<dynamic>();
    }
}
