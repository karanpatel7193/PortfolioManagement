CREATE PROCEDURE [dbo].[User_SelectForRecord]
	@Id bigint
AS
/***********************************************************************************************
	 NAME     :  User_SelectForRecord
	 PURPOSE  :  This SP select perticular record from table User for open page in edit / view mode.
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        02/06/2018					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	SELECT U.[Id], U.[FirstName], U.[MiddleName], U.[LastName], U.[RoleId], U.[Username], U.[Password], U.[PasswordSalt], U.[Email], U.[PhoneNumber], U.[ImageSrc], U.[IsActive], U.[LastUpdateDateTime], U.[Gender] 
	FROM [User] U WITH (NOLOCK)
	WHERE U.[Id] = @Id
END


