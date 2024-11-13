CREATE PROCEDURE [dbo].[AccountBroker_SelectForRecord]
    @Id INT
AS
BEGIN
    SELECT 
        [Id], 
        [BrokerId], 
        [AccountId]
    FROM [dbo].[AccountBroker]
    WHERE [Id] = @Id;
END
GO
