CREATE PROCEDURE [dbo].[Broker_Update]
    @Id TINYINT,
    @Name VARCHAR(20),
    @BrokerTypeId INT,
    @BuyBrokerage FLOAT = NULL,
    @SellBrokerage FLOAT = NULL
AS

BEGIN
    -- Check if another record with the same Name and BrokerTypeId exists, excluding the current record
    IF NOT EXISTS (
        SELECT [Id]
        FROM [dbo].[Broker]
        WHERE [Name] = @Name
        AND [BrokerTypeId] = @BrokerTypeId
        AND [Id] = @Id
    )
    BEGIN
        -- Update the existing record with the specified Id
        UPDATE [dbo].[Broker]
        SET 
            [Name] = @Name,
            [BrokerTypeId] = @BrokerTypeId,
            [BuyBrokerage] = @BuyBrokerage,
            [SellBrokerage] = @SellBrokerage
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