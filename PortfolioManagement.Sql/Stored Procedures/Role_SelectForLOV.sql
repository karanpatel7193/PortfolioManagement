/*
This SP select records from table Role for bind LOV
Created By :: Rekansh Patel
Created On :: 04/23/2017
*/	
CREATE PROCEDURE [dbo].[Role_SelectForLOV]
	@IsPublic BIT = NULL
AS
BEGIN
	SELECT [Id], [Name] 
	FROM [Role]  
	WHERE IsPublic = COALESCE(@IsPublic, IsPublic) AND [Id] > 1;
END

