CREATE PROCEDURE [dbo].[AccountBroker_SelectForPortfolioDatewiseReport]
AS
BEGIN
    SELECT 
        AB.[BrokerId],
        AB.[AccountId],
        A.[PmsId]
    FROM [dbo].[AccountBroker] AB

    INNER JOIN  [Account] A
    ON          AB.AccountId = A.Id


END
