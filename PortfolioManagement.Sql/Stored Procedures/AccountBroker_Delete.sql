CREATE PROCEDURE [dbo].[AccountBroker_Delete]
    @Id         INT
AS
BEGIN
    DELETE FROM   [dbo].[AccountBroker]
    WHERE       [Id] = @Id;
END
