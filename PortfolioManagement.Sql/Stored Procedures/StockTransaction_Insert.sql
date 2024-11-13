
CREATE  PROCEDURE [dbo].[StockTransaction_Insert]
    @Date				DATETIME = NULL,
    @AccountId			TINYINT,
    @TransactionTypeId	INT,
    @ScriptId			SMALLINT,
    @Qty				SMALLINT,
    @Price				FLOAT,
    @BrokerId			TINYINT,
    @Brokerage			FLOAT,
    @Buy				FLOAT,
    @Sell				FLOAT,
    @Dividend			FLOAT,
	@PmsId				INT

AS
BEGIN
    BEGIN TRANSACTION StockTransactionInsert
	BEGIN TRY
    -- Insert a new record into the Transaction table
		DECLARE @Id		INT
		IF NOT EXISTS(SELECT [Id] FROM [StockTransaction] WHERE [Date]=@Date AND [AccountId]=@AccountId AND [TransactionTypeId]=@TransactionTypeId AND [ScriptId]=@ScriptId AND [Qty]=@Qty AND [Price]=@Price AND [BrokerId]=@BrokerId AND [Brokerage]=@Brokerage AND [Buy]=@Buy AND [Sell]=@Sell AND [Dividend]=@Dividend )
		BEGIN
			INSERT INTO [StockTransaction] ([Date],		[AccountId],	[TransactionTypeId],	[ScriptId],		[Qty],		[Price],		[BrokerId],		[Brokerage],		[Buy],		[Sell],		[Dividend],	[PmsId])
						   VALUES          ( @Date,		@AccountId,		@TransactionTypeId,		@ScriptId,		@Qty,		@Price,			@BrokerId,		@Brokerage,			@Buy,		 @Sell,		@Dividend,	@PmsId)
			SET @Id = SCOPE_IDENTITY();
			EXEC [dbo].[Portfolio_Insert_Update_Inner] @Id 
													   , @Date	
													   , @AccountId		
													   , @TransactionTypeId
													   , @ScriptId		
													   , @Qty			
													   , @Price			
													   , @BrokerId		
													   , @Brokerage		
													   , @Buy			
													   , @Sell
													   , @PmsId
													   
		END
		ELSE  
			SET @Id = 0;
	
		SELECT @Id;
		IF @@TRANCOUNT > 0
			COMMIT TRANSACTION StockTransactionInsert
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION StockTransactionInsert
		DECLARE @ErrMsg nvarchar(4000), @ErrSeverity int
		SELECT @ErrMsg = ERROR_MESSAGE(), @ErrSeverity = ERROR_SEVERITY();
		RAISERROR(@ErrMsg, @ErrSeverity, 1)
	END CATCH
END
