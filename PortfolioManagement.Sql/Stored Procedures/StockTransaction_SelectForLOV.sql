CREATE PROCEDURE [dbo].[StockTransaction_SelectForLOV]
    @Id BIGINT = NULL,
    @Date DATETIME = NULL,
    @AccountId TINYINT = NULL,
    @TransactionTypeId TINYINT = NULL,
    @ScriptId SMALLINT = NULL,
    @BrokerId TINYINT = NULL
AS
BEGIN
    SELECT 
        T.[Id],        
        T.[Date],      
        T.[AccountId],        
        T.[TransactionTypeId],        
        T.[ScriptId],        
        T.[BrokerId]        

    FROM 
        [StockTransaction] T
    WHERE 
        T.[Id] = COALESCE(@Id, T.[Id])
        AND T.[Date] = COALESCE(@Date, T.[Date])
        AND T.[AccountId] = COALESCE(@AccountId, T.[AccountId])
        AND T.[TransactionTypeId] = COALESCE(@TransactionTypeId, T.[TransactionTypeId])
        AND T.[ScriptId] = COALESCE(@ScriptId, T.[ScriptId])
        AND T.[BrokerId] = COALESCE(@BrokerId, T.[BrokerId])
        
END