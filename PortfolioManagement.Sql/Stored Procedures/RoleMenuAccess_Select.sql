/*
This SP select records from table RoleMenuAccess
Created By :: Rekansh Patel
Created On :: 05/27/2017
*/
CREATE PROCEDURE [dbo].[RoleMenuAccess_Select]
	@IsMenu BIT = NULL
AS
BEGIN
	SELECT RA.[Id], RA.[RoleId], 
		RA.[MenuId], 
		RA.[CanInsert], 
		RA.[CanUpdate], 
		RA.[CanDelete], 
		RA.[CanView] 
	FROM [RoleMenuAccess] RA
		INNER JOIN dbo.Menu M ON RA.MenuId = M.Id AND M.IsMenu = COALESCE(@IsMenu, M.IsMenu)
END

