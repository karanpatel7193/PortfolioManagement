/*
This SP select records from table User
Created By :: Rekansh Patel
Created On :: 04/23/2017
*/	
CREATE PROCEDURE [dbo].[User_SelectPasswordSalt]
	@Username nvarchar(50) 
AS
BEGIN
	SELECT U.[PasswordSalt]
	FROM [User]  U WITH (NOLOCK) 
	WHERE (U.[Email] = @Username OR U.[Username] = @Username)
END



