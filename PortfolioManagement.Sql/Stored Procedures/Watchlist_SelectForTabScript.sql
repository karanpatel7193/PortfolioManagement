
CREATE PROCEDURE [dbo].[Watchlist_SelectForTabScript]
    @WatchlistId        INT,
    @PmsId              INT
AS
BEGIN
    -- Set NOCOUNT ON to prevent extra result sets from interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Select script and day summary data for the given watchlist
    SELECT 
        WS.[WatchListId], 
        WS.[ScriptId],
		WS.[Id],
        S.[Name],
        S.[NseCode],
        SLS.[Open],
        SLS.[Close],
        SLS.[High],
        SLS.[Low],
        SLS.[Volume],
        SLS.Price,
        SLS.[High52Week],
        SLS.[Low52Week]
    FROM [dbo].[WatchlistScript] WS WITH (NOLOCK)
    INNER JOIN [dbo].[Script] S WITH (NOLOCK) 
        ON S.[Id] = WS.[ScriptId]
    LEFT JOIN [dbo].[ScriptLatestValue] SLS WITH (NOLOCK)
        ON SLS.[ScriptId] = S.[Id] 
    WHERE WS.[WatchListId] = @WatchlistId
        AND WS.PmsId = @PmsId;
END;