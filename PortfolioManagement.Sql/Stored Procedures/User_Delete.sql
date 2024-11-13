/*
This SP delete record from table User
Created By :: Rekansh Patel
Created On :: 04/23/2017
*/	
CREATE PROCEDURE [dbo].[User_Delete]
	@Id bigint
AS
BEGIN
	DELETE FROM [User] 
	WHERE [Id] = @Id;
END

