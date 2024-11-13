
CREATE PROCEDURE [dbo].[Portfolio_SelectForPortfolioDatewiseProcess]
    @AccountId	TINYINT, 
    @BrokerId	TINYINT,
	@PmsId		INT		
AS
BEGIN

	SELECT			SUM(PO.InvestmentValue) AS InvestmentAmount, 
					SUM(PO.MarketValue)		AS UnReleasedAmount
	FROM			( 
					SELECT
						P.[AccountId],  
						P.[ScriptId],  
						P.[BrokerId],
						ROUND((S.[Price] * P.Qty), 2) AS MarketValue,
						ROUND((P.CostPrice * P.Qty), 2) AS InvestmentValue
					FROM
						(
							SELECT
								PII.[AccountId],  
								PII.[ScriptId],  
								PII.[BrokerId],  
								SUM(PII.[BuyQty] - PII.[SellQty]) AS Qty,
								ROUND(AVG(CASE ISNULL(PII.[BuyQty], 0) WHEN 0 THEN 0 ELSE PII.[BuyAmount] / PII.[BuyQty] END), 2) AS CostPrice
							FROM
								[Portfolio] PII
							WHERE
									PII.[PmsId]						= @PmsId
								AND PII.[BuyQty] - PII.[SellQty]	!= 0
								AND	PII.[AccountId]					= @AccountId
								AND PII.[BrokerId]					= @BrokerId
							GROUP BY
								PII.[AccountId],  
								PII.[ScriptId],  
								PII.[BrokerId]
						) P
					INNER JOIN	[Script]	S 
							ON	P.[ScriptId] = S.[Id]
						) PO
END;
