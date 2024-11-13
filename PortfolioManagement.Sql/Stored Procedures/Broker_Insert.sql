CREATE PROCEDURE [dbo].[Broker_Insert]
    @Id             TINYINT = NULL,  -- Optional Id parameter
    @Name           VARCHAR(20) = NULL, 
    @BrokerTypeId   INT, 
    @BuyBrokerage   FLOAT, 
    @SellBrokerage  FLOAT,
	@PmsId          INT = NULL 
AS
BEGIN
    -- If Id is not provided, generate a new one based on the max existing Id
    IF @Id IS NULL
    BEGIN
        SELECT @Id = ISNULL(MAX([Id]), 0) + 1 FROM [dbo].[Broker];
    END

    -- Insert the new record into the Broker table
    INSERT INTO [dbo].[Broker] ([Id], [Name], [BrokerTypeId], [BuyBrokerage], [SellBrokerage], [PmsId])
    VALUES (@Id, @Name, @BrokerTypeId, @BuyBrokerage, @SellBrokerage , @PmsId);

    -- Return the Id of the newly inserted record
    SELECT @Id;
END;