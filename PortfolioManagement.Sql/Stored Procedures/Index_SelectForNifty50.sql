CREATE PROCEDURE [dbo].[Index_SelectForNifty50]
AS
BEGIN
    SELECT 
        SP.[NseCode], 
        SLV.[Price],
        (SLV.[Price] - SLV.[PreviousDay]) AS PriceChange,
        ((SLV.[Price] - SLV.[PreviousDay]) / SLV.[PreviousDay]) * 100 AS PricePercentage
    FROM 
        [ScriptLatestValue] SLV
    INNER JOIN [Script] SP 
        ON SP.Id = SLV.ScriptId
    WHERE 
        SP.IsNifty50 = 1 
END;
