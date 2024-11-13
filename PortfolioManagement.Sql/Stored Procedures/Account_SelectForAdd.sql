CREATE PROCEDURE [dbo].[Account_SelectForAdd]
    @Id         INT,
    @PmsId      INT
AS

BEGIN
    SELECT  
                    B.[Id], 
                    B.[Name],
                    CONVERT(BIT, CASE WHEN AB.Id IS NULL THEN 0 ELSE 1 END) AS IsSelected
    FROM            [dbo].[Broker] B
        LEFT JOIN   [dbo].[AccountBroker] AB
            ON      AB.[BrokerId]   = B.Id
            AND     AB.[AccountId]  = @Id
            WHERE   B.[PmsId]       = @PmsId
END