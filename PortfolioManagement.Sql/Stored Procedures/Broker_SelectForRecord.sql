CREATE PROCEDURE [dbo].[Broker_SelectForRecord]
    @Id TINYINT
AS
BEGIN
    SELECT B.[Id], B.[Name], B.[BrokerTypeId], B.[BuyBrokerage], B.[SellBrokerage]
    FROM [dbo].[Broker] B
    WHERE B.[Id] = @Id;
END