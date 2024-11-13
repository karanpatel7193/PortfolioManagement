CREATE PROCEDURE [dbo].[PortfolioDatewise_SelectForPortfolioDatewiseReport]
    @PmsId          INT,
    @BrokerId       TINYINT = NULL,
    @AccountId      TINYINT = NULL,
    @FromDate       DATE,
    @ToDate         DATE
AS
BEGIN
    SELECT 
        PD.[Date],
        SUM(PD.[InvestmentAmount]) AS TotalInvestmentAmount,
        SUM(PD.[UnReleasedAmount]) AS TotalUnReleasedAmount
    FROM 
        [PortfolioDateWise] PD
    WHERE 
        PD.[PmsId]      = @PmsId
        AND PD.[Date]   >= @FromDate
        AND PD.[Date]   <= @ToDate
        AND (@BrokerId  IS NULL OR PD.[BrokerId]    = @BrokerId)
        AND (@AccountId IS NULL OR PD.[AccountId]   = @AccountId)
    GROUP BY 
        PD.[Date]
    ORDER BY 
        PD.[Date];
END
