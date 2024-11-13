CREATE PROCEDURE [dbo].[Broker_SelectForLov]
    @Id TINYINT = NULL, 
    @Name VARCHAR(10) = NULL,
    @PmsId INT
AS

BEGIN
    SELECT 
        [Id], 
        [Name]
    FROM [dbo].[Broker]
    WHERE 
        [Id] = COALESCE(@Id, [Id])
        AND [Name] = COALESCE(@Name, [Name])
        AND [PmsId] = @PmsId;
        
END