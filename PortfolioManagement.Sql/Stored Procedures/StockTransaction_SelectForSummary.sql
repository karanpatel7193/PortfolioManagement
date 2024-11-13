Create  PROCEDURE [dbo].[StockTransaction_SelectForSummary]
    @AccountId          INT = NULL,
    @BrokerId           INT = NULL,
    @TransactionTypeId  INT = NULL,
    @ScriptId           INT = NULL,
    @FromDate           DATE = NULL,
    @ToDate             DATE = NULL,
    @PmsId              INT
AS
BEGIN
    SELECT
        T.[AccountId],
        A.[Name] AS AccountName,
        SUM(T.[Buy]) AS Buy,
        SUM(T.[Sell]) AS Sell,
        SUM(T.[Dividend]) AS Dividend
    FROM
        dbo.[StockTransaction] T
    INNER JOIN dbo.[Account] A 
        ON T.[AccountId] = A.[Id]
    WHERE
        T.[AccountId] = COALESCE(@AccountId, T.[AccountId])
        AND T.[BrokerId] = COALESCE(@BrokerId, T.[BrokerId])
        AND T.[TransactionTypeId] = COALESCE(@TransactionTypeId, T.[TransactionTypeId])
        AND T.[ScriptId] = COALESCE(@ScriptId, T.[ScriptId])
        AND (T.[Date] >= COALESCE(@FromDate, T.[Date]))
        AND (T.[Date] <= COALESCE(@ToDate, T.[Date]))
        AND T.[PmsId] = @PmsId

    GROUP BY
        T.[AccountId], A.[Name];
END;



--EXEC [dbo].[StockTransaction_SelectForSummary]
--    @AccountId = NULL,
--    @BrokerId = NULL,
--    @TransactionTypeId = NULL,
--    @ScriptId = NULL,
--    @FromDate = NULL,
--    @ToDate = NULL;



	