/*
This SP select records from table User
Created By :: Rekansh Patel
Created On :: 04/23/2017
*/	
CREATE PROCEDURE [dbo].[User_SelectValidLogin]
	@Username nvarchar(250), 
	@Password varchar(max),
	@Now DATETIME 
AS
BEGIN
	DECLARE @UserId BIGINT
	DECLARE @UserLoginLogId BIGINT
	
	SELECT @UserId = U.[Id] 
	FROM [User]  U WITH (NOLOCK) 
	WHERE	(U.[Email] = @Username OR U.[Username] = @Username)
		AND U.[Password] = @Password
		AND U.IsActive = 1

	SELECT U.[Id], U.[FirstName], U.[LastName], U.[Email], U.[RoleId], U.[Username], R.[Name] AS RoleName, U.ImageSrc, @UserLoginLogId AS UserLoginLogId , U.[PmsId], P.[Name] AS PmsName
	FROM [User]  U WITH (NOLOCK) 
		INNER JOIN [Role]  R ON U.RoleId = R.Id
		INNER JOIN [PMS]   P ON U.PmsId	 = P.Id
	WHERE	U.Id = @UserId
END

