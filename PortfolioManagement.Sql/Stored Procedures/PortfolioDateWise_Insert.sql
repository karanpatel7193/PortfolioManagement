CREATE PROCEDURE [dbo].[PortfolioDateWise_Insert]
    @PmsId INT,
    @BrokerId TINYINT,
    @AccountId TINYINT,
    @Date DATE,
    @InvestmentAmount FLOAT = NULL,
    @UnReleasedAmount FLOAT = NULL
AS
BEGIN
    DECLARE @Id INT;

    -- Check if a record with the same PmsId, BrokerId, AccountId, and Date exists
    IF EXISTS (SELECT [Id] FROM [PortfolioDateWise] WHERE [PmsId] = @PmsId AND [BrokerId] = @BrokerId AND [AccountId] = @AccountId AND [Date] = @Date)
    BEGIN
        -- Update existing record
        UPDATE [PortfolioDateWise]
        SET [InvestmentAmount] = @InvestmentAmount,
            [UnReleasedAmount] = @UnReleasedAmount
        WHERE [PmsId] = @PmsId AND [BrokerId] = @BrokerId AND [AccountId] = @AccountId AND [Date] = @Date;

        -- Get the ID of the updated record
        SET @Id = (SELECT [Id] FROM [PortfolioDateWise] WHERE [PmsId] = @PmsId AND [BrokerId] = @BrokerId AND [AccountId] = @AccountId AND [Date] = @Date);
    END
    ELSE
    BEGIN
        -- Insert new record
        INSERT INTO [PortfolioDateWise] ([PmsId], [BrokerId], [AccountId], [Date], [InvestmentAmount], [UnReleasedAmount])
        VALUES (@PmsId, @BrokerId, @AccountId, @Date, @InvestmentAmount, @UnReleasedAmount);
        
        -- Get the ID of the newly inserted record
        SET @Id = SCOPE_IDENTITY();
    END

    -- Return the ID (ID of updated or newly inserted record)
    SELECT @Id AS NewId;
END
