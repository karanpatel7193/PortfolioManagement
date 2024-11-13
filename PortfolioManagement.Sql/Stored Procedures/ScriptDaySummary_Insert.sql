CREATE PROCEDURE [dbo].[ScriptDaySummary_Insert]
	@ScriptId smallint, 
	@Date date, 
	@PreviousDay float, 
	@Open float, 
	@Close float, 
	@High float, 
	@Low float, 
	@DateTime datetime, 
	@Price float,
    @Volume bigint, 
    @Value float, 
    @High52Week float, 
    @Low52Week float
AS
/***********************************************************************************************
	 NAME     :  ScriptDaySummary_Insert
	 PURPOSE  :  This SP insert record in table ScriptDaySummary.
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        11/22/2020					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	BEGIN TRANSACTION ScriptDaySummaryInsert
	BEGIN TRY 
		UPDATE		[Script]	 
		SET			[Price]			=		@Price,
					[PreviousDay]	=		@PreviousDay
		WHERE		[Id]	= @ScriptId

		DECLARE @Id bigint
		IF NOT EXISTS(SELECT [Id] FROM [ScriptPrice] WHERE [ScriptId] = @ScriptId AND [DateTime] = @DateTime AND [Volume] = @Volume )
		BEGIN
			INSERT INTO [ScriptPrice] ([ScriptId], [DateTime], [Price], [Volume], [PreviousDay], [Open], [Close], [High], [Low]) 
			VALUES (@ScriptId, @DateTime, @Price, @Volume, @PreviousDay, @Open, @Close, @High, @Low)

			SET @Id = SCOPE_IDENTITY();
		END

		IF NOT EXISTS(SELECT [Id] FROM [ScriptDaySummary] WHERE [ScriptId] = @ScriptId AND [Date] = @Date)
			INSERT INTO [ScriptDaySummary] ([ScriptId], [Date], [Price], [PreviousDay], [Open], [Close], [High], [Low], [Volume], [Value], [High52Week], [Low52Week]) 
			VALUES (@ScriptId, @Date, @Price, @PreviousDay, @Open, @Close, @High, @Low, @Volume, @Value, @High52Week, @Low52Week)
		ELSE
			UPDATE	[ScriptDaySummary] 
			SET		[Price]			= @Price,
					[PreviousDay]	= @PreviousDay, 
					[Open]			= @Open, 
					[Close]			= @Close, 
					[High]			= @High, 
					[Low]			= @Low,
					[Volume]		= @Volume,
					[Value]			= @Value,
					[High52Week]	= @High52Week,
					[Low52Week]		= @Low52Week
			WHERE	[ScriptId]	= @ScriptId 
				AND [Date]		= @Date;

		IF NOT EXISTS(SELECT [Id] FROM [ScriptLatestValue] WHERE [ScriptId] = @ScriptId)
			INSERT INTO [ScriptLatestValue] ([ScriptId], [Price], [PreviousDay], [Open], [Close], [High], [Low], [Volume], [Value], [High52Week], [Low52Week]) 
			VALUES (@ScriptId, @Price, @PreviousDay, @Open, @Close, @High, @Low, @Volume, @Value, @High52Week, @Low52Week)
		ELSE
			UPDATE	[ScriptLatestValue] 
			SET		[Price]			= @Price,
					[PreviousDay]	= @PreviousDay, 
					[Open]			= @Open, 
					[Close]			= @Close, 
					[High]			= @High, 
					[Low]			= @Low,
					[Volume]		= @Volume,
					[Value]			= @Value,
					[High52Week]	= @High52Week,
					[Low52Week]		= @Low52Week
			WHERE	[ScriptId]	= @ScriptId 
	
		SELECT @Id;
		IF @@TRANCOUNT > 0
			COMMIT TRANSACTION ScriptDaySummaryInsert
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION ScriptDaySummaryInsert
		
		DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT, @ErrorNumber INT
		SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE(), @ErrorNumber = ERROR_NUMBER()
		RAISERROR  (@ErrorMessage,@ErrorSeverity,@ErrorState,@ErrorNumber)
	END CATCH
END