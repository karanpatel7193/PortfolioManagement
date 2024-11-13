CREATE  PROCEDURE [dbo].[StockTransaction_SelectForReport]
    @AccountId INT = NULL,
    @BrokerId INT = NULL,
    @TransactionTypeId INT = NULL,
    @ScriptId INT = NULL,
    @FromDate DATE = NULL,
    @ToDate DATE = NULL,
    @PmsId INT
AS
BEGIN
    SELECT
           T.[Id],        
           T.[Date],      
           T.[AccountId],        
           T.[TransactionTypeId],        
           T.[ScriptId],        
           T.[BrokerId],        
           T.[Brokerage],        
           T.[Qty],
           T.[Price],        
           T.[Buy],        
           T.[Sell],        
           T.[Dividend],
           A.[Name] AS AccountName,
           TT.[ValueText] AS TransactionTypeName,
           S.[NseCode] AS ScriptName,
           B.[Name] AS BrokerName
    FROM
        dbo.[StockTransaction] T
    INNER JOIN dbo.[Account] A 
        ON T.[AccountId] = A.[Id]
    INNER JOIN dbo.[MasterValue] TT 
        ON T.[TransactionTypeId] = TT.[Value]
    INNER JOIN dbo.[Script] S 
        ON T.[ScriptId] = S.[Id]
    INNER JOIN dbo.[Broker] B 
        ON T.[BrokerId] = B.[Id]
    WHERE
        T.[AccountId] = COALESCE(@AccountId, T.[AccountId])
        AND T.[BrokerId] = COALESCE(@BrokerId, T.[BrokerId])
        AND T.[TransactionTypeId] = COALESCE(@TransactionTypeId, T.[TransactionTypeId])
        AND T.[ScriptId] = COALESCE(@ScriptId, T.[ScriptId])
        AND (T.[Date] >= COALESCE(@FromDate, T.[Date]))
        AND (T.[Date] <= COALESCE(@ToDate, T.[Date]))
        AND T.[PmsId] = @PmsId;

END;
