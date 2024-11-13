CREATE  PROCEDURE [dbo].[StockTransaction_Delete]
	@Id			INT
AS
BEGIN
	 DECLARE @AccountId				TINYINT,
             @ScriptId				SMALLINT,
             @BrokerId				TINYINT,
			 @TransactionTypeId		INT,
             @DateTime				DATETIME

    SELECT @AccountId			= AccountId,
           @ScriptId			= ScriptId,
           @BrokerId			= BrokerId,
           @TransactionTypeId	= TransactionTypeId,
           @DateTime			= [Date]
    FROM	[dbo].[StockTransaction]
    WHERE	Id					= @Id;
	 
	BEGIN TRANSACTION StockTransactionDelete 
	BEGIN TRY
		EXEC [dbo].[Portfolio_Insert_Update]	 @AccountId
												,@ScriptId
												,@BrokerId
												,@DateTime 
												,1
												,@Id
												,@TransactionTypeId

		IF @@TRANCOUNT > 0
			COMMIT TRANSACTION StockTransactionDelete
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION StockTransactionDelete
		DECLARE @ErrMsg nvarchar(4000), @ErrSeverity int
		SELECT @ErrMsg = ERROR_MESSAGE(), @ErrSeverity = ERROR_SEVERITY();
		RAISERROR(@ErrMsg, @ErrSeverity, 1)
	END CATCH


END