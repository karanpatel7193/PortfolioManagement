CREATE PROCEDURE [dbo].[Watchlist_Delete]
    @Id INT  -- ID of the Watchlist to be deleted
AS
BEGIN
    -- Start a transaction
    BEGIN TRANSACTION WatchlistDelete;

    BEGIN TRY
        -- Delete associated scripts from WatchlistScript table
        DELETE FROM [dbo].[WatchlistScript]
        WHERE [WatchListId] = @Id;

        -- Delete the Watchlist from Watchlist table
        DELETE FROM [dbo].[Watchlist]
        WHERE [Id] = @Id;

        -- Commit the transaction if no errors occur
        IF @@TRANCOUNT > 0
            COMMIT TRANSACTION WatchlistDelete;
    END TRY
    BEGIN CATCH
        -- Rollback the transaction in case of an error
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION WatchlistDelete;

        -- Error handling
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;