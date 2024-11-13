CREATE PROCEDURE [dbo].[Menu_SelectForGrid]
	@Name varchar(50) = NULL, 
	@ParentId int = NULL, 
	@SortExpression varchar(50),
	@SortDirection varchar(5),
	@PageIndex int,
	@PageSize int
AS
/***********************************************************************************************
	 NAME     :  Menu_SelectForGrid
	 PURPOSE  :  This SP select records from table Menu for bind Menu page grid.
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        02/03/2018					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	DECLARE @SqlQuery NVARCHAR(MAX)

	SET @SqlQuery = N'
	SELECT M.[Id], M.[Name], M.[Description], M.[ParentId], M.[PageTitle], M.[Icon], M.[Routing], M.[OrderBy], M.[IsMenu],
		P.Name AS ParentIdName
	FROM [Menu] M
		LEFT JOIN [Menu] P ON M.ParentId = P.Id
	WHERE  M.[Name] like COALESCE(@Name, M.[Name]) + ''%''
		AND (@ParentId IS NULL OR M.[ParentId] = @ParentId)
	ORDER BY ' + @SortExpression + ' ' + @SortDirection + '
	OFFSET ((@PageIndex - 1) * @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY
	'

	EXEC sp_executesql @SqlQuery, N'@Name varchar(50), @ParentId int = NULL, @PageIndex int, @PageSize int', @Name, @ParentId, @PageIndex, @PageSize
	
	SELECT COUNT(1) AS TotalRecords
	FROM [Menu] M
	WHERE  M.[Name] like COALESCE(@Name, M.[Name]) + '%'
	 AND (@ParentId IS NULL OR M.[ParentId] = @ParentId)

END

