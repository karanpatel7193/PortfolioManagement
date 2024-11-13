CREATE PROCEDURE [dbo].[User_SelectForGrid]
	@FirstName nvarchar(50)				= NULL, 
	@LastName nvarchar(50)				= NULL, 
	@Email				nvarchar(250)	= NULL,
	@RoleId				smallint		= NULL, 
	@PhoneNumber		VARCHAR(15)		= NULL, 
	@Username			nvarchar(50)	= NULL, 
	@SortExpression		varchar(50),
	@SortDirection		varchar(5),
	@PageIndex			int,
	@PageSize			int,
	@PmsId				int
AS
/***********************************************************************************************
	 NAME     :  User_SelectForGrid
	 PURPOSE  :  This SP select records from table User for bind User page grid.
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        02/06/2018					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	DECLARE @SqlQuery NVARCHAR(MAX)

	SET @SqlQuery = N'
	SELECT U.[Id], U.[FirstName], U.[MiddleName], U.[LastName], U.[RoleId], U.[Username], U.[Email], U.[PhoneNumber], R.NAME AS RoleName, U.IsActive 
	FROM [User] U WITH (NOLOCK) 
		INNER JOIN [Role]  R ON U.RoleId = R.Id
	WHERE	 U.[FirstName] LIKE COALESCE(@FirstName, U.[FirstName]) + ''%''
		 AND U.[LastName] LIKE COALESCE(@LastName, U.[LastName]) + ''%''
		 AND U.[PmsId] = COALESCE(@PmsId, U.[PmsId])
		 AND (@Email IS NULL OR U.[Email] LIKE @Email + ''%'')
		 AND U.[RoleId] = COALESCE(@RoleId, U.[RoleId])
		 AND (@PhoneNumber IS NULL OR U.[PhoneNumber] LIKE @PhoneNumber + ''%'')
		 AND U.[Username] LIKE COALESCE(@Username, U.[Username]) + ''%''
	ORDER BY ' + @SortExpression + ' ' + @SortDirection + '
	OFFSET ((@PageIndex - 1) * @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY
	'

	EXEC sp_executesql @SqlQuery, N'@FirstName nvarchar(50), @LastName nvarchar(50), @Email nvarchar(250), @RoleId smallint, @PhoneNumber VARCHAR(15), @Username nvarchar(50), @PageIndex int, @PageSize int, @PmsId int', 
						@FirstName, @LastName, @Email, @RoleId, @PhoneNumber, @Username, @PageIndex, @PageSize, @PmsId
	
	SELECT COUNT(1) AS TotalRecords
	FROM [User] U WITH (NOLOCK) 
	WHERE	 U.[FirstName] LIKE COALESCE(@FirstName, U.[FirstName]) + '%'
		 AND U.[LastName] LIKE COALESCE(@LastName, U.[LastName]) + '%'
		 AND (@Email IS NULL OR U.[Email] LIKE @Email + '%')
		 AND U.[RoleId] = COALESCE(@RoleId, U.[RoleId])
		 AND U.[PmsId] = COALESCE(@PmsId, U.[PmsId])
		 AND (@PhoneNumber IS NULL OR U.[PhoneNumber] LIKE @PhoneNumber + '%')
		 AND U.[Username] LIKE COALESCE(@Username, U.[Username]) + '%'

END
