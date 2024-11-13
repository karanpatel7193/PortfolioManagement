CREATE PROCEDURE [dbo].[PortfolioDateWise_Update]
    @PmsId INT,
    @BrokerId TINYINT,
    @AccountId TINYINT,
    @Date DATE,
    @InvestmentAmount FLOAT = NULL,
    @UnReleasedAmount FLOAT = NULL
AS
BEGIN
    -- Update the record based on the provided parameters
    UPDATE [PortfolioDateWise]
    SET 
        InvestmentAmount = @InvestmentAmount,
        UnReleasedAmount = @UnReleasedAmount
    WHERE 
        PmsId = @PmsId AND 
        BrokerId = @BrokerId AND 
        AccountId = @AccountId AND 
        [Date] = @Date;

    -- Optionally, return the number of affected rows
    SELECT @@ROWCOUNT AS AffectedRows;
END
