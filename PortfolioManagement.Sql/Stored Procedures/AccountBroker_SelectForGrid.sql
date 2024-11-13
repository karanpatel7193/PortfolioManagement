CREATE PROCEDURE [dbo].[AccountBroker_SelectForGrid]
    @BrokerId           TINYINT = NULL,
    @AccountId          TINYINT = NULL,
    @SortExpression     VARCHAR(50),
    @SortDirection      VARCHAR(5),
    @PageIndex          INT,
    @PageSize           INT
AS
BEGIN
    DECLARE @SqlQuery NVARCHAR(MAX);

    -- Construct the dynamic SQL query
    SET @SqlQuery = N'
    SELECT 
        [Id], 
        [BrokerId], 
        [AccountId]
    FROM [dbo].[AccountBroker]
    WHERE 
        [BrokerId] = COALESCE(@BrokerId, [BrokerId])
        AND [AccountId] = COALESCE(@AccountId, [AccountId])
        ORDER BY ' + @SortExpression + ' ' + @SortDirection + '
        OFFSET ((@PageIndex - 1) * @PageSize) ROWS
        FETCH NEXT @PageSize ROWS ONLY;
        ';

    -- Execute the dynamic SQL query
  	EXEC sp_executesql @SqlQuery, N'@BrokerId TINYINT, @AccountId TINYINT, @PageIndex INT, @PageSize INT',
                                     @BrokerId, @AccountId, @PageIndex, @PageSize;

    -- Get the total number of records
    SELECT COUNT(1) AS TotalRecords
    FROM [dbo].[AccountBroker]
    WHERE 
        [BrokerId] = COALESCE(@BrokerId, [BrokerId])
        AND [AccountId] = COALESCE(@AccountId, [AccountId]);
END
GO
