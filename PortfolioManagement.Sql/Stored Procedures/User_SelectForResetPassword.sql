/*
This SP select records from table User using user name or email address
Created By :: Rekansh Patel
Created On :: 08/05/2018
*/	
CREATE PROCEDURE [dbo].[User_SelectForResetPassword]
	@UsernameEmail nvarchar(250) = NULL 
AS
BEGIN
	SELECT U.[Id], U.[FirstName], U.[LastName], U.[Email], U.[Username]
	FROM [User]  U WITH (NOLOCK) 
	WHERE 
		(	U.[Email]		=	COALESCE(@UsernameEmail, U.[Email])
		OR	U.[Username]	=	COALESCE(@UsernameEmail, U.[Username]))
END




