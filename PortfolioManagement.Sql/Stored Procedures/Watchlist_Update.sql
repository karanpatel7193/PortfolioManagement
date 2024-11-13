
CREATE PROCEDURE [dbo].[Watchlist_Update]
    @Id INT,                   -- ID of the Watchlist to be updated
    @Name NVARCHAR(255),      -- New Name to update
    @PmsId INT                -- New PMS ID to update
AS
BEGIN
    IF NOT EXISTS (SELECT Id FROM [dbo].[Watchlist] WHERE [Name] = @Name AND [Id] = @Id AND [PmsId] = @PmsId)
    BEGIN
        -- Update the Watchlist table
        UPDATE [dbo].[Watchlist]
        SET 
            [Name] = @Name,
            [PmsId] = @PmsId
        WHERE [Id] = @Id;

        -- Return the updated Id
        SELECT @Id;
    END
    ELSE
    BEGIN
        -- Return 0 to indicate no update
        SELECT 0;
    END
END

EXEC [dbo].[Watchlist_Update]
    @Id = 7,                   
    @Name = 'Updated Name',   
    @PmsId = 2;