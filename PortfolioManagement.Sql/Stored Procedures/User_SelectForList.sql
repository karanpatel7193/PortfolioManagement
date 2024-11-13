CREATE PROCEDURE [dbo].[User_SelectForList]
	@OrganizationName		nvarchar(100)	= NULL, 
	@FirstName				nvarchar(50)	= NULL, 
	@LastName				nvarchar(50)	= NULL, 
	@Email					nvarchar(250)	= NULL,
	@RoleId					smallint		= NULL, 
	@PhoneNumber			VARCHAR(15)		= NULL, 
	@Username				nvarchar(50)	= NULL, 
	@ParentUserId			BIGINT			= NULL,
	@SortExpression			varchar(50),
	@SortDirection			varchar(5),
	@PageIndex				int,
	@PageSize				int,
	@PmsId					int
AS
/***********************************************************************************************
	 NAME     :  User_SelectForList
	 PURPOSE  :  This SP use for fill all LOV and list grid in User list page
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        02/06/2018					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	EXEC Role_SelectForLOV 

	EXEC User_SelectForGrid @FirstName, @LastName, @Email, @RoleId, @PhoneNumber, @Username, @SortExpression, @SortDirection, @PageIndex, @PageSize, @PmsId
END

