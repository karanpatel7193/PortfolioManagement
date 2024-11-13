/*
This SP delete record from table Menu
Created By :: Rekansh Patel
Created On :: 05/27/2017
*/
CREATE PROCEDURE [dbo].[Menu_Delete]
	@Id int
AS
BEGIN
	BEGIN TRANSACTION Menu_Delete
	BEGIN TRY
		DELETE FROM [RoleMenuAccess] 
		WHERE MenuId = @Id

		DELETE FROM [Menu]
		WHERE [Id] = @Id;
			 
		IF @@TRANCOUNT > 0
			COMMIT TRANSACTION Menu_Delete
	END TRY
	BEGIN CATCH			
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION Menu_Delete

		DECLARE @ErrMsg nvarchar(4000), @ErrSeverity int
		SELECT @ErrMsg = ERROR_MESSAGE(), @ErrSeverity = ERROR_SEVERITY();
		RAISERROR(@ErrMsg, @ErrSeverity, 1)
	END CATCH

END

