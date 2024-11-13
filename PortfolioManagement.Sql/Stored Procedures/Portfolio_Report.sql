CREATE PROCEDURE [dbo].[Portfolio_Report]
    @AccountId	TINYINT = NULL, 
    @BrokerId	TINYINT = NULL,
	@PmsId		INT		
AS
BEGIN
	SELECT
		P.[AccountId],  
		A.[Name] AS AccountName,  
		P.[ScriptId],  
		S.[NseCode] AS ScriptName,
		S.[IndustryName],
		P.[BrokerId],  
		B.[Name] AS BrokerName,
		P.Qty,
		P.CostPrice,
		S.[Price] AS CurrentPrice,
		S.[PreviousDay] AS PreviousDayPrice,
		(
			SELECT		SUM(ST.Dividend)
			FROM		dbo.StockTransaction ST 
			WHERE		ST.[AccountId]			= P.[AccountId] 
					AND ST.[BrokerId]			= P.[BrokerId] 
					AND ST.[PmsId]				= @PmsId 
					AND ST.[ScriptId]			= P.[ScriptId]
					AND ST.[TransactionTypeId]	= 3003
		) +
		(
			SELECT		SUM(PIR.SellAmount - (CASE ISNULL(PIR.[BuyQty],0) WHEN 0 THEN 0 ELSE PIR.SellQty * PIR.BuyAmount / PIR.BuyQty END))
			FROM		[Portfolio] PIR 
			WHERE		PIR.[AccountId]			= P.[AccountId] 
					AND PIR.[BrokerId]			= P.[BrokerId] 
					AND PIR.[PmsId]				= @PmsId 
					AND PIR.[ScriptId]			= P.[ScriptId]
					AND PIR.[SellQty]			> 0
		) AS ReleasedProfit
	
	FROM
		(
			SELECT
				PII.[AccountId],  
				PII.[ScriptId],  
				PII.[BrokerId],  
				SUM(PII.[BuyQty] - PII.[SellQty]) AS Qty,
				ROUND(AVG(CASE ISNULL(PII.[BuyQty],0) WHEN 0 THEN 0 ELSE PII.[BuyAmount] / PII.[BuyQty] END), 2) AS CostPrice
			FROM
				[Portfolio] PII
			WHERE
					PII.[PmsId]						= @PmsId
				AND PII.[BuyQty] - PII.[SellQty]	!= 0
				AND	(@AccountId			IS NULL OR PII.[AccountId]	= @AccountId)
				AND (@BrokerId			IS NULL OR PII.[BrokerId]	= @BrokerId)
			GROUP BY
				PII.[AccountId],  
				PII.[ScriptId],  
				PII.[BrokerId]
		) P
	INNER JOIN	[Script]	S 
			ON	P.[ScriptId] = S.[Id]
	INNER JOIN	[Account]	A 
			ON	P.[AccountId] = A.[Id]
	INNER JOIN	[Broker]	B 
			ON	P.[BrokerId] = B.[Id]
	ORDER BY
			S.[Name];
END;
