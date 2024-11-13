/*
This SP select records from table RoleMenuAccess
Created By :: Rekansh Patel
Created On :: 05/27/2017
*/
CREATE PROCEDURE [dbo].[RoleMenuAccess_SelectByRoleIdParentId]
	@RoleId smallint,
	@ParentId int
AS
BEGIN
	DECLARE @COUNT INT
	SELECT @COUNT = COUNT(A.Id) FROM [Menu] A WHERE A.ParentId = @ParentId
	
	SELECT RA.[Id], RA.[RoleId], 
		A.Id AS [MenuId], 
		ISNULL(RA.[CanInsert],0) AS CanInsert, 
		ISNULL(RA.[CanUpdate],0) AS CanUpdate, 
		ISNULL(RA.[CanDelete],0) AS CanDelete, 
		ISNULL(RA.[CanView],0) AS CanView, 
		A.Name AS MenuIdName
	FROM [Menu] A
		LEFT JOIN [RoleMenuAccess] RA ON A.Id = RA.MenuId AND RA.[RoleId] = @RoleId
	WHERE  A.IsPublic = 0 
		AND (
				(A.ParentId = @ParentId)
			OR  (@COUNT = 0 AND A.Id = @ParentId)
			)
	ORDER BY A.OrderBy
END

