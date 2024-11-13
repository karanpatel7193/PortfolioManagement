CREATE PROCEDURE [dbo].[AccountBroker_Select]
    @Id INT = NULL,
    @BrokerId TINYINT = NULL,
    @AccountId TINYINT = NULL
AS
BEGIN
    SELECT 
        AB.[Id],
        AB.[BrokerId],
        AB.[AccountId]
    FROM [dbo].[AccountBroker] AB
    WHERE 
        AB.[Id] = COALESCE(@Id, AB.[Id])
        AND AB.[BrokerId] = COALESCE(@BrokerId, AB.[BrokerId])
        AND AB.[AccountId] = COALESCE(@AccountId, AB.[AccountId]);
END
