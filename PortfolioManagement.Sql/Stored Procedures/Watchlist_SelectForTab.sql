CREATE PROCEDURE [dbo].[Watchlist_SelectForTab]
    @PmsId INT
AS
BEGIN
    SELECT 
        [Id], 
        [Name]
    FROM [dbo].[Watchlist]
    WHERE 
        [PmsId] = @PmsId
END