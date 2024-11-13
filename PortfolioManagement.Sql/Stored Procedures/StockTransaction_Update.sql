CREATE  PROCEDURE [dbo].[StockTransaction_Update]
    @Id					INT = NULL,
    @Date				DATETIME,
    @AccountId			TINYINT,
    @TransactionTypeId	INT,
    @ScriptId			SMALLINT,
    @Qty				SMALLINT,
    @Price				FLOAT,
    @BrokerId			TINYINT = NULL,
    @Brokerage			FLOAT	= NULL,
    @Buy				FLOAT	= NULL,
    @Sell				FLOAT	= NULL,
    @Dividend			FLOAT	= NULL
AS
BEGIN

    SELECT @AccountId		= AccountId,
           @ScriptId		= ScriptId,
           @BrokerId		= BrokerId,
           @Date			= [Date]
    FROM	[dbo].[StockTransaction]
    WHERE	Id				= @Id;

	BEGIN TRANSACTION StockTransactionUpdate 
	BEGIN TRY
		UPDATE	dbo.[StockTransaction]
		SET		 [Date]					= @Date				
				,AccountId				= @AccountId			
				,TransactionTypeId		= @TransactionTypeId	
				,ScriptId				= @ScriptId			
				,Qty					= @Qty				
				,Price					= @Price				
				,BrokerId				= @BrokerId			
				,Brokerage				= @Brokerage			
				,Buy					= @Buy				
				,Sell					= @Sell				
				,Dividend				= @Dividend			
		WHERE	 Id						= @Id

		EXEC [dbo].[Portfolio_Insert_Update]	 @AccountId
												,@ScriptId
												,@BrokerId
												,@Date 
												,0
												,@Id
												,@TransactionTypeId
		SELECT 1;


		IF @@TRANCOUNT > 0
			COMMIT TRANSACTION StockTransactionUpdate
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION StockTransactionUpdate
		DECLARE @ErrMsg nvarchar(4000), @ErrSeverity int
		SELECT @ErrMsg = ERROR_MESSAGE(), @ErrSeverity = ERROR_SEVERITY();
		RAISERROR(@ErrMsg, @ErrSeverity, 1)
	END CATCH
END
