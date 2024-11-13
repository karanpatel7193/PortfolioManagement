CREATE PROCEDURE [dbo].[Account_SelectForGrid]
    @Name              NVARCHAR(50) = NULL,
    @SortExpression     VARCHAR(50),
    @SortDirection      VARCHAR(5),
    @PageIndex          INT,
    @PageSize           INT,
    @PmsId              INT
AS
BEGIN
    SET NOCOUNT ON; -- Improves performance by preventing extra result sets

    DECLARE @SqlQuery NVARCHAR(MAX);

    -- Construct the dynamic SQL query
    SET @SqlQuery = N'
    SELECT 
        [Id], 
        [Name]
    FROM [dbo].[Account] A WITH (NOLOCK)
    WHERE 
        [Name] LIKE COALESCE(@Name, [Name]) + ''%''
    AND 
        [PmsId] = @PmsId
    ORDER BY ' + QUOTENAME(@SortExpression) + ' ' + @SortDirection + '
    OFFSET ((@PageIndex - 1) * @PageSize) ROWS
    FETCH NEXT @PageSize ROWS ONLY;';

    -- Execute the dynamic SQL query
    EXEC sp_executesql @SqlQuery, 
                       N'@Name NVARCHAR(50), @PmsId INT, @PageIndex INT, @PageSize INT',
                       @Name, @PmsId, @PageIndex, @PageSize;

    -- Get the total number of records
    SELECT COUNT(1) AS TotalRecords
    FROM [dbo].[Account] A WITH (NOLOCK)
    WHERE 
        [Name] LIKE COALESCE(@Name, [Name]) + '%'
    AND 
        [PmsId] = @PmsId;
END
GO
