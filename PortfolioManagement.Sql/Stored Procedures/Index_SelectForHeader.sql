CREATE PROCEDURE [dbo].[Index_SelectForHeader]

AS
BEGIN
    SELECT TOP 1
        I.[Nifty],
		(I.[Nifty] - I.[NiftyPreviousDay]) AS NiftyDiff,
        ROUND(((I.[Nifty] - I.[NiftyPreviousDay]) / I.[NiftyPreviousDay]) * 100, 2) AS NiftyPercentage,
         I.[Sensex],
        ROUND(((I.[Sensex] - I.[SensexPreviousDay]) / I.[SensexPreviousDay]) * 100,2 ) AS SensexPercentage,
		(I.[Sensex] - I.[SensexPreviousDay]) AS SensexDiff
    FROM 
        [Index] I
    ORDER BY I.Id DESC
END;