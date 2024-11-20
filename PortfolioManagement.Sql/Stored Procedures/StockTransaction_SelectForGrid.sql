CREATE PROCEDURE [dbo].[StockTransaction_SelectForGrid]
    @Id BIGINT = NULL,
    @Date DATETIME = NULL,
    @AccountId TINYINT = NULL,
    @TransactionTypeId INT = NULL,
    @ScriptId SMALLINT = NULL,
    @BrokerId TINYINT = NULL,
    @SortExpression VARCHAR(50),
    @SortDirection VARCHAR(5),
    @FromDate DATE = NULL,
    @ToDate DATE = NULL,
    @PageIndex INT,
    @PageSize INT,
    @PmsId INT
AS
BEGIN
    DECLARE @SqlQuery NVARCHAR(MAX);

    -- Construct the SQL query
    SET @SqlQuery = N'
    SELECT T.[Id],        
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
    FROM [StockTransaction] T
    INNER JOIN dbo.[Account] A 
        ON T.[AccountId] = A.[Id]
    INNER JOIN dbo.[MasterValue] TT 
        ON T.[TransactionTypeId] = TT.[Value]
    INNER JOIN dbo.[Script] S 
        ON T.[ScriptId] = S.[Id]
    INNER JOIN dbo.[Broker] B 
        ON T.[BrokerId] = B.[Id]
    WHERE T.[Id] = COALESCE(@Id, T.[Id])
      AND T.[Date] = COALESCE(@Date, T.[Date])
      AND T.[AccountId] = COALESCE(@AccountId, T.[AccountId])
      AND T.[TransactionTypeId] = COALESCE(@TransactionTypeId, T.[TransactionTypeId])
      AND T.[ScriptId] = COALESCE(@ScriptId, T.[ScriptId])
      AND T.[BrokerId] = COALESCE(@BrokerId, T.[BrokerId])
      AND (T.[Date] >= COALESCE(@FromDate, T.[Date]))
      AND (T.[Date] <= COALESCE(@ToDate, T.[Date]))
      AND T.[PmsId] = @PmsId
    ORDER BY ' + QUOTENAME(@SortExpression) + ' ' + @SortDirection + '
    OFFSET ((@PageIndex - 1) * @PageSize) ROWS
    FETCH NEXT @PageSize ROWS ONLY
    ';

    -- Execute the dynamic SQL with all required parameters
    EXEC sp_executesql 
        @SqlQuery,
        N'@Id BIGINT, @Date DATETIME, @AccountId TINYINT, @TransactionTypeId INT, @ScriptId SMALLINT, @BrokerId TINYINT, @PageIndex INT, @PageSize INT, @PmsId INT, @FromDate DATE, @ToDate DATE',
        @Id = @Id, 
        @Date = @Date, 
        @AccountId = @AccountId, 
        @TransactionTypeId = @TransactionTypeId, 
        @ScriptId = @ScriptId, 
        @BrokerId = @BrokerId, 
		@FromDate = @FromDate,
		@ToDate	  = @ToDate,	
        @PageIndex = @PageIndex, 
        @PageSize = @PageSize,
        @PmsId = @PmsId;

    -- Get the total count of records
    SELECT COUNT(1) AS TotalRecords
    FROM [StockTransaction] T
    WHERE T.[Id] = COALESCE(@Id, T.[Id])
      AND T.[Date] = COALESCE(@Date, T.[Date])
      AND T.[AccountId] = COALESCE(@AccountId, T.[AccountId])
      AND T.[TransactionTypeId] = COALESCE(@TransactionTypeId, T.[TransactionTypeId])
      AND T.[ScriptId] = COALESCE(@ScriptId, T.[ScriptId])
      AND T.[BrokerId] = COALESCE(@BrokerId, T.[BrokerId])
      AND (T.[Date] >= COALESCE(@FromDate, T.[Date]))
      AND (T.[Date] <= COALESCE(@ToDate, T.[Date]))
      AND T.[PmsId] = COALESCE(@PmsId, T.[PmsId]);
      
END