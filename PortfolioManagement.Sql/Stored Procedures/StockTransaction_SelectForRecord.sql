CREATE PROCEDURE [dbo].[StockTransaction_SelectForRecord]
    @Id BIGINT
AS
BEGIN
    -- Select the record with the specified Id
    SELECT 
        T.[Id],        
        T.[Date],      
        T.[AccountId],        
        T.[TransactionTypeId],        
        T.[ScriptId],        
        T.[Qty],
        T.[Price],        
        T.[BrokerId],        
        T.[Brokerage],        
        T.[Buy],        
        T.[Sell],        
        T.[Dividend]
    FROM 
        [StockTransaction] T
    WHERE 
        T.[Id] = @Id;
END
