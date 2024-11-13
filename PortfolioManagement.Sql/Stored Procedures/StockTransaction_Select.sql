CREATE  PROCEDURE [dbo].[StockTransaction_Select]
    @Id						BIGINT = NULL,
    @Date					DATETIME = NULL,
    @AccountId				TINYINT = NULL,
    @TransactionTypeId		INT = NULL,
    @ScriptId				SMALLINT = NULL,
    @Qty					SMALLINT = NULL,
    @Price					FLOAT = NULL,
    @BrokerId				TINYINT = NULL,
    @Brokerage				FLOAT = NULL,
    @Buy					FLOAT = NULL,
    @Sell					FLOAT = NULL,
    @Dividend				FLOAT = NULL
AS
BEGIN
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
        (@Id IS NULL OR T.[Id] = @Id)
        AND (@Date IS NULL OR T.[Date] = @Date)
        AND (@AccountId IS NULL OR T.[AccountId] = @AccountId)
        AND (@TransactionTypeId IS NULL OR T.[TransactionTypeId] = @TransactionTypeId)
        AND (@ScriptId IS NULL OR T.[ScriptId] = @ScriptId)
        AND (@Qty IS NULL OR T.[Qty] = @Qty)
        AND (@Price IS NULL OR T.[Price] = @Price)
        AND (@BrokerId IS NULL OR T.[BrokerId] = @BrokerId)
        AND (@Brokerage IS NULL OR T.[Brokerage] = @Brokerage)
        AND (@Buy IS NULL OR T.[Buy] = @Buy)
        AND (@Sell IS NULL OR T.[Sell] = @Sell)
        AND (@Dividend IS NULL OR T.[Dividend] = @Dividend)
END
