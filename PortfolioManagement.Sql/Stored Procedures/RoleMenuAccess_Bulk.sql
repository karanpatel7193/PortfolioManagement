/*
This SP bulk insert/update records in table RoleMenuAccess
Created By :: Rekansh Patel
Created On :: 05/27/2017
*/
CREATE PROCEDURE [dbo].[RoleMenuAccess_Bulk]
	@RoleId smallint,
	@AccessXML xml
AS
BEGIN
	BEGIN TRANSACTION RoleMenuAccessBulk
	BEGIN TRY
		DECLARE @DocHandle int
		EXEC sp_xml_preparedocument @DocHandle OUTPUT, @AccessXML

		INSERT INTO [RoleMenuAccess]([RoleId], [MenuId], [CanInsert], [CanUpdate], [CanDelete], [CanView])
		SELECT @RoleId, [MenuId], [CanInsert], [CanUpdate], [CanDelete], [CanView]
		FROM OPENXML (@DocHandle, 'ArrayOfRoleMenuAccessEntity/RoleMenuAccessEntity',2)
		WITH ([Id] INT, [MenuId] INT, [CanInsert] BIT, [CanUpdate] BIT, [CanDelete] BIT, [CanView] BIT)
		WHERE [Id] = 0 AND ([CanInsert] = 1 OR [CanUpdate] = 1 OR [CanDelete] = 1 OR [CanView] = 1)

		UPDATE RoleMenuAccess 
		SET [CanInsert] = raXML.[CanInsert], 
			[CanUpdate] = raXML.[CanUpdate], 
			[CanDelete] = raXML.[CanDelete], 
			[CanView] = raXML.[CanView]
		FROM RoleMenuAccess RA 
			INNER JOIN (
				SELECT [Id], [MenuId], [CanInsert], [CanUpdate], [CanDelete], [CanView]
				FROM OPENXML (@DocHandle, 'ArrayOfRoleMenuAccessEntity/RoleMenuAccessEntity',2)
				WITH ([Id] INT, [MenuId] INT, [CanInsert] BIT, [CanUpdate] BIT, [CanDelete] BIT, [CanView] BIT)
				WHERE [Id] != 0
			) raXML ON RA.Id = raXML.Id

		UPDATE RoleMenuAccess 
		SET [CanView] = RAO.[CanView]
		FROM RoleMenuAccess RA 
			INNER JOIN (
				SELECT AD.ParentId, RAD.RoleId, CanView = CASE COUNT(1) WHEN 0 THEN 0 ELSE 1 END
				FROM Menu AD
					INNER JOIN RoleMenuAccess RAD ON RAD.MenuId = AD.Id AND RAD.RoleId = @RoleId
				WHERE AD.ParentId IS NOT NULL AND (RAD.[CanInsert] = 1 OR RAD.[CanUpdate] = 1 OR RAD.[CanDelete] = 1 OR RAD.[CanView] = 1) 
				GROUP BY AD.ParentId, RAD.RoleId
			) RAO ON RA.MenuId = RAO.ParentId AND RA.RoleId = RAO.RoleId

		DELETE FROM dbo.RoleMenuAccess 
		WHERE MenuId IN (
				SELECT ParentId FROM dbo.Menu WHERE ParentId IS NOT NULL GROUP BY ParentId
				)
			 AND RoleId = @RoleId 
		DELETE FROM dbo.RoleMenuAccess WHERE ([CanInsert] = 0 AND [CanUpdate] = 0 AND [CanDelete] = 0 AND [CanView] = 0) AND RoleId = @RoleId 

		INSERT INTO RoleMenuAccess (MenuId, RoleId, CanView, CanInsert, CanUpdate, CanDelete)  
		SELECT CHILD.ParentId, CHILD.RoleId, 1, 0, 0, 0
		FROM ( 
			SELECT AD.ParentId, RAD.RoleId
			FROM Menu AD
				INNER JOIN RoleMenuAccess RAD ON RAD.MenuId = AD.Id AND RAD.RoleId = @RoleId 
			WHERE AD.ParentId IS NOT NULL AND (RAD.[CanInsert] = 1 OR RAD.[CanUpdate] = 1 OR RAD.[CanDelete] = 1 OR RAD.[CanView] = 1)
			GROUP BY AD.ParentId, RAD.RoleId
			) CHILD 
			LEFT JOIN RoleMenuAccess RA ON CHILD.ParentId = RA.MenuId AND CHILD.RoleId = RA.RoleId
		WHERE RA.Id IS NULL
					 
		EXEC sp_xml_removedocument @DocHandle

		IF @@TRANCOUNT > 0
			COMMIT TRANSACTION RoleMenuAccessBulk
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION RoleMenuAccessBulk
		
		EXEC sp_xml_removedocument @DocHandle

		DECLARE @ErrMsg nvarchar(4000), @ErrSeverity int
		SELECT @ErrMsg = ERROR_MESSAGE(), @ErrSeverity = ERROR_SEVERITY();
		RAISERROR(@ErrMsg, @ErrSeverity, 1)
	END CATCH
END

