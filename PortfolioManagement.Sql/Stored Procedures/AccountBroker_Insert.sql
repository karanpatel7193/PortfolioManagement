CREATE PROCEDURE [dbo].[AccountBroker_Insert]
    @BrokerId       TINYINT,
    @AccountId      TINYINT
AS
BEGIN
    DECLARE @Id INT;

    -- Check if a record with the same BrokerId and AccountId already exists
    IF NOT EXISTS (
        SELECT  [Id]
        FROM    [dbo].[AccountBroker]
        WHERE   [BrokerId] = @BrokerId AND [AccountId] = @AccountId
    )
    BEGIN
        -- Insert a new record into the AccountBroker table
        INSERT INTO [dbo].[AccountBroker] ([BrokerId], [AccountId])
        VALUES (@BrokerId, @AccountId);
        
        -- Get the ID of the newly inserted record
        SET @Id = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        -- If the record exists, set the ID to 0
        SET @Id = 0;
    END
    
    -- Return the ID of the record
    SELECT @Id;
END

