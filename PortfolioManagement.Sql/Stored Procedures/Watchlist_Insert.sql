
CREATE PROCEDURE [dbo].[Watchlist_Insert]
    @Name NVARCHAR(255),
    @PmsId INT
AS
BEGIN
    -- Insert into Watchlist table
    INSERT INTO [Watchlist] ([Name], [PmsId])
    VALUES (@Name, @PmsId);

    -- Return the newly created WatchlistId
    SELECT SCOPE_IDENTITY() AS WatchlistId;
END;