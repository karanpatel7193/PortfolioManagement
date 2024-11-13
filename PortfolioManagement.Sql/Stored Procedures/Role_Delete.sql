/*
This SP delete record from table Role
Created By :: Rekansh Patel
Created On :: 04/23/2017
*/	
CREATE PROCEDURE [dbo].[Role_Delete]
	@Id smallint
AS
BEGIN
	DELETE FROM [Role] 
	WHERE [Id] = @Id;
END

