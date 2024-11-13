/*
This SP select records from table User
Created By :: Rekansh Patel
Created On :: 04/23/2017
*/	
CREATE PROCEDURE [dbo].[User_SelectForChangePassword]
	@Id bigint = NULL
AS
BEGIN
	SELECT U.[Password], U.[PasswordSalt]
	FROM [User]  U WITH (NOLOCK) 
	WHERE	U.[Id] = @Id
END

