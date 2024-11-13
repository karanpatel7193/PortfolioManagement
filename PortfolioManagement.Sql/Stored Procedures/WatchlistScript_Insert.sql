CREATE PROCEDURE [dbo].[WatchlistScript_Insert]
    @WatchListId INT,  
    @ScriptId SMALLINT,
    @PmsId INT

AS
BEGIN
	 -- Check if the combination of WatchListId and ScriptId already exists
    IF NOT EXISTS (
        SELECT Id 
        FROM [dbo].[WatchlistScript] 
        WHERE [WatchListId] = @WatchListId 
          AND [ScriptId] = @ScriptId
    )
        -- Insert into WatchlistScript table
        INSERT INTO [dbo].[WatchlistScript] ([WatchListId], [ScriptId],[PmsId])
        VALUES (@WatchListId, @ScriptId, @PmsId);
END;