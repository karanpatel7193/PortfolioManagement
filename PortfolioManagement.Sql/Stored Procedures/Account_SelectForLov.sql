CREATE PROCEDURE [dbo].[Account_SelectForLOV]
    --@AccountId TINYINT = NULL
    @Id TINYINT = NULL, 
    @Name VARCHAR(10) = NULL,
    @PmsId INT
AS
BEGIN
    -- Fetch all brokers
    SELECT  A.[Id],   A.[Name]
    FROM    [dbo].[Account] A
     WHERE 
            [Id] = COALESCE(@Id, [Id])
        AND [Name] = COALESCE(@Name, [Name])
        AND [PmsId] = @PmsId;

END

--EXEC [dbo].[Account_SelectForLOV] @AccountId = 1;

    --SELECT 
    --    B.Id AS BrokerId,
    --    B.Name AS BrokerName,
    --    ISNULL(AB.AccountId, 0) AS IsSelected -- 0 if not selected, otherwise the AccountId
    --FROM
    --    dbo.[Broker] B
    --    LEFT JOIN dbo.[AccountBroker] AB ON AB.BrokerId = B.Id AND AB.AccountId = @AccountId
    --ORDER BY
    --    B.Name ASC;