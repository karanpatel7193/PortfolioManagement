/*
This SP select records from table User
Created By :: Rekansh Patel
Created On :: 04/23/2017
*/	
CREATE PROCEDURE [dbo].[User_Select]
	@Id bigint = NULL, 
	@FirstName nvarchar(50) = NULL, 
	@LastName nvarchar(50) = NULL, 
	@RoleId smallint = NULL, 
	@Email nvarchar(250) = NULL, 
	@Username nvarchar(50) = NULL, 
	@Password varchar(max) = NULL, 
	@PasswordSalt varchar(max) = NULL 
AS
BEGIN
	SELECT U.[Id], U.[FirstName], U.[MiddleName], U.[LastName], U.[RoleId], U.[Email], U.[PhoneNumber], U.[Username], U.[Password], U.[PasswordSalt], R.[Name] AS RoleName , U.IsActive, U.LastUpdateDateTime
	FROM [User]  U  WITH (NOLOCK) 
		INNER JOIN [Role]  R ON U.RoleId = R.Id
	WHERE U.[Id] = COALESCE(@Id, U.[Id])
	 AND U.[FirstName] = COALESCE(@FirstName, U.[FirstName])
	 AND U.[LastName] = COALESCE(@LastName, U.[LastName])
	 AND U.[RoleId] = COALESCE(@RoleId, U.[RoleId])
	 AND U.[Email] = COALESCE(@Email, U.[Email])
	 AND U.[Username] = COALESCE(@Username, U.[Username])
	 AND U.[Password] = COALESCE(@Password, U.[Password])
	 AND U.[PasswordSalt] = COALESCE(@PasswordSalt, U.[PasswordSalt]) 
END

