CREATE PROCEDURE [dbo].[StockTransaction_SelectForList]
@PmsId INT
AS
BEGIN
    EXEC [dbo].[Account_SelectForLOV] @PmsId = @PmsId

    SELECT      B.[Id], B.[Name], B.[BrokerTypeId], B.[BuyBrokerage], B.[SellBrokerage], AB.[AccountId]
    FROM        [dbo].[Broker] B
        INNER JOIN [AccountBroker] AB 
            ON  B.Id = AB.BrokerId
    WHERE       [PmsId] = @PmsId;

    EXEC [dbo].[Script_SelectForLOV]

END
GO