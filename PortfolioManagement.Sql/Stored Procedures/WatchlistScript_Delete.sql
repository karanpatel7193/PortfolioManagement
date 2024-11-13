CREATE PROCEDURE [dbo].[WatchlistScript_Delete]
    @Id INT -- ID of the record to be deleted from WatchlistScript
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the record exists before trying to delete
    IF EXISTS (SELECT Id FROM [dbo].[WatchlistScript] WHERE [Id] = @Id)
    BEGIN
        -- Delete the record
        DELETE FROM [dbo].[WatchlistScript]
        WHERE [Id] = @Id;

        PRINT 'Record deleted successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Record not found.';
    END
END;