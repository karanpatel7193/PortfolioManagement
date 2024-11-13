/*
This SP select records from table Role
Created By :: Rekansh Patel
Created On :: 04/23/2017
*/	
CREATE PROCEDURE [dbo].[Role_Select]
	@Id smallint = NULL, 
	@Name varchar(50) = NULL,
	@IsPublic BIT = NULL 
AS
BEGIN
	SELECT [Id], [Name], IsPublic 
	FROM [Role]  
	WHERE [Id] = COALESCE(@Id, [Id])
		AND [Name] = COALESCE(@Name, [Name]) 
		AND IsPublic = COALESCE(@IsPublic, IsPublic)
END

