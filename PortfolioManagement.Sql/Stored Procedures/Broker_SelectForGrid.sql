CREATE PROCEDURE [dbo].[Broker_SelectForGrid]
    @Name               VARCHAR(20)       = NULL,
    @BrokerTypeId       INT               = NULL,
    @SortExpression     VARCHAR(50),
    @SortDirection      VARCHAR(5),
    @PageIndex          INT,
    @PageSize           INT,
    @PmsId              INT
AS

BEGIN
    DECLARE @SqlQuery NVARCHAR(MAX)

    SET @SqlQuery = N'
    SELECT B.[Id],
           B.[Name],
           BT.[ValueText] AS BrokerType,
           B.[BrokerTypeId],
           B.[BuyBrokerage],
           B.[SellBrokerage]
    FROM        [dbo].[Broker] B
        INNER JOIN dbo.[MasterValue] BT 
                ON B.[BrokerTypeId] = BT.[Value]
    WHERE   B.[Name] LIKE COALESCE(@Name, B.[Name]) + ''%'' 
        AND B.[BrokerTypeId] = COALESCE(@BrokerTypeId, B.[BrokerTypeId]) 
        AND B.[PmsId] = @PmsId
    ORDER BY ' + @SortExpression + ' ' + @SortDirection + '
    OFFSET ((@PageIndex - 1) * @PageSize) ROWS
    FETCH NEXT @PageSize ROWS ONLY
    '

    EXEC sp_executesql @SqlQuery,
        N'@Name VARCHAR(20), @BrokerTypeId TINYINT, @PageIndex INT, @PageSize INT, @PmsId INT',
        @Name, @BrokerTypeId, @PageIndex, @PageSize, @PmsId

    SELECT COUNT(1) AS TotalRecords
    FROM        [dbo].[Broker] B
    WHERE   B.[Name] LIKE COALESCE(@Name, B.[Name]) + '%'
        AND B.[BrokerTypeId] = COALESCE(@BrokerTypeId, B.[BrokerTypeId])
        AND B.[PmsId] = @PmsId

END