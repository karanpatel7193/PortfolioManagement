CREATE PROCEDURE [dbo].[Master_Delete]
    @Id SMALLINT
AS
BEGIN
    BEGIN TRANSACTION MasterDelete
    BEGIN TRY 
        -- Delete from MasterValue table where MasterId matches
        DELETE FROM [dbo].[MasterValue]
        WHERE [MasterId] = @Id;

        -- Delete from Master table where Id matches
        DELETE FROM [dbo].[Master]
        WHERE [Id] = @Id;

        -- Commit the transaction if no errors occur
        IF @@TRANCOUNT > 0
            COMMIT TRANSACTION MasterDelete
    END TRY
    BEGIN CATCH
        -- Rollback the transaction in case of an error
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION MasterDelete
        
        -- Error handling
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE()
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)
    END CATCH
END
