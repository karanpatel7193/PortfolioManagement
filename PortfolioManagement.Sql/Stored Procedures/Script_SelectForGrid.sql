CREATE   PROCEDURE [dbo].[Script_SelectForGrid]
    @Name varchar(30) = null, 
    @BseCode numeric(6,0) = null, 
    @NseCode varchar(30) = null, 
    @SortExpression varchar(50),
    @SortDirection varchar(5),
    @PageIndex int,
    @PageSize int
AS
BEGIN
    DECLARE @SqlQuery NVARCHAR(MAX)

    -- Construct the dynamic SQL query
    SET @SqlQuery = N'
    SELECT S.[Id], S.[Name], S.[BseCode], S.[NseCode], S.[ISINCode], S.[MoneyControlURL], S.[FetchURL], S.[IsFetch], S.[IsActive], S.[Price], S.[IndustryName], S.[FaceValue], S.[Group]
    FROM [Script] S
    WHERE   S.[Name] = COALESCE(@Name, S.[Name])
        AND S.[BseCode] = COALESCE(@BseCode, S.[BseCode])
        AND S.[NseCode] = COALESCE(@NseCode, S.[NseCode])
    ORDER BY ' + QUOTENAME(@SortExpression) + ' ' + @SortDirection + '
    OFFSET ((@PageIndex - 1) * @PageSize) ROWS
    FETCH NEXT @PageSize ROWS ONLY
    '

    -- Execute the dynamic SQL query
    EXEC sp_executesql @SqlQuery, 
        N'@Name varchar(30), @BseCode numeric(6,0), @NseCode varchar(30), @PageIndex int, @PageSize int',
        @Name, @BseCode, @NseCode, @PageIndex, @PageSize

    -- Get the total number of records
    SELECT COUNT(1) AS TotalRecords
    FROM [Script] S
    WHERE   S.[Name] = COALESCE(@Name, S.[Name])
        AND S.[BseCode] = COALESCE(@BseCode, S.[BseCode])
        AND S.[NseCode] = COALESCE(@NseCode, S.[NseCode])
END		
	