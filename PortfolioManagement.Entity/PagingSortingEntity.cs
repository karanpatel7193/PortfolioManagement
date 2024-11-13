namespace PortfolioManagement.Entity
{
    public class PagingSortingEntity
    {
        public PagingSortingEntity()
        {
            this.SortExpression = string.Empty;
            this.SortDirection = string.Empty;
            this.PageIndex = 1;
            this.PageSize = 10;
            this.TotalRecords = 0;
        }

        public string SortExpression { get; set; }
        public string SortDirection { get; set; }
        public long PageIndex { get; set; }
        public int PageSize { get; set; }
        public long TotalRecords { get; set; }
    }
}
