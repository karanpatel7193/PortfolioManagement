CREATE PROCEDURE [dbo].[Account_Delete]
    @Id TINYINT
AS
BEGIN
    BEGIN TRANSACTION AccountDelete
    BEGIN TRY 
        -- Delete from AccountBroker where AccountId matches
        DELETE FROM [dbo].[AccountBroker]
        WHERE [AccountId] = @Id;

        -- Delete from Account table where Id matches
        DELETE FROM [dbo].[Account]
        WHERE [Id] = @Id;

        -- Commit the transaction if no errors occur
        IF @@TRANCOUNT > 0
            COMMIT TRANSACTION AccountDelete
    END TRY
    BEGIN CATCH
        -- Rollback the transaction in case of an error
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION AccountDelete
        
        -- Error handling
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE()
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)
    END CATCH
END

--EXEC [dbo].[Account_Delete] @Id = 1;
