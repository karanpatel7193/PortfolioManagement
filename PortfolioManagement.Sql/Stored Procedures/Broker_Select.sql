CREATE PROCEDURE [dbo].[Broker_Select]
    @Id TINYINT = NULL, 
    @Name VARCHAR(20) = NULL, 
    @BrokerTypeId INT = NULL, 
    @BuyBrokerage FLOAT(53) = NULL, 
    @SellBrokerage FLOAT(53) = NULL,
    @IsActive BIT = NULL
AS
/***********************************************************************************************
    NAME     :  SelectBroker
    PURPOSE  :  This SP selects records from the Broker table based on optional filter parameters.
    REVISIONS:
    Ver        Date        Author            Description
    ---------  ----------  ----------------  -------------------------------
    1.0        03/09/2024  Rekansh Patel     1. Initial Version.	 
************************************************************************************************/
BEGIN
    SELECT 
        [Id], 
        [Name], 
        [BrokerTypeId], 
        [BuyBrokerage], 
        [SellBrokerage]
    FROM [dbo].[Broker]
    WHERE 
        [Id] = COALESCE(@Id, [Id])
        AND [Name] = COALESCE(@Name, [Name])
        AND [BrokerTypeId] = COALESCE(@BrokerTypeId, [BrokerTypeId])
        AND [BuyBrokerage] = COALESCE(@BuyBrokerage, [BuyBrokerage])
        AND [SellBrokerage] = COALESCE(@SellBrokerage, [SellBrokerage])
END