CREATE PROCEDURE [dbo].[PMS_SelectForGrid]
    @Name VARCHAR(255) = NULL, 
    @SortExpression VARCHAR(50),
    @SortDirection VARCHAR(5),
    @PageIndex INT,
    @PageSize INT
AS
BEGIN
    DECLARE @SqlQuery NVARCHAR(MAX)

    SET @SqlQuery = N'
    SELECT P.[Id], P.[Name], P.[IsActive], P.[Type]
    FROM [PMS] P
    WHERE P.[Name] LIKE COALESCE(@Name, P.[Name]) + ''%''
    ORDER BY ' + QUOTENAME(@SortExpression) + ' ' + @SortDirection + '
    OFFSET ((@PageIndex - 1) * @PageSize) ROWS
    FETCH NEXT @PageSize ROWS ONLY
    '

    EXEC sp_executesql @SqlQuery, 
                        N'@Name VARCHAR(255), @PageIndex INT, @PageSize INT', 
                        @Name, @PageIndex, @PageSize
    
    SELECT COUNT(1) AS TotalRecords
    FROM [PMS] P
    WHERE P.[Name] LIKE COALESCE(@Name, P.[Name]) + '%'

END
