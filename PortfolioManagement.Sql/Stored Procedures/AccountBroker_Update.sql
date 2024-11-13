CREATE PROCEDURE [dbo].[AccountBroker_Update]
    @Id INT,
    @BrokerId TINYINT,
    @AccountId TINYINT
AS
BEGIN
    -- Check if another record with the same BrokerId and AccountId exists, excluding the current record
    IF NOT EXISTS (
        SELECT [Id]
        FROM [dbo].[AccountBroker]
        WHERE [BrokerId] = @BrokerId
        AND [AccountId] = @AccountId
        AND [Id] != @Id
    )
    BEGIN
        -- Update the existing record with the specified Id
        UPDATE [dbo].[AccountBroker]
        SET 
            [BrokerId] = @BrokerId,
            [AccountId] = @AccountId
        WHERE [Id] = @Id;
    END
    ELSE
    BEGIN
        -- If a duplicate record exists, set the Id to 0
        SET @Id = 0;
    END

    -- Return the Id of the updated record or 0 if a duplicate was found
    SELECT @Id;
END