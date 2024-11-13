CREATE  PROCEDURE   [dbo].[Broker_Delete]
    @BrokerId TINYINT
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        -- Delete the broker from the Broker table
        DELETE FROM [dbo].[Broker]
        WHERE [Id] = @BrokerId;

        -- Commit the transaction
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Rollback the transaction in case of error
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Raise the error to the caller
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;