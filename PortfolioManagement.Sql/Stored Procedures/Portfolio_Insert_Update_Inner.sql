CREATE PROCEDURE [dbo].[Portfolio_Insert_Update_Inner]
	@TransactionId		INT,
    @Date				DATETIME = NULL,
    @AccountId			TINYINT,
    @TransactionTypeId	INT,
    @ScriptId			SMALLINT,
    @Qty				SMALLINT,
    @Price				FLOAT,
    @BrokerId			TINYINT = NULL,
    @Brokerage			FLOAT	= NULL,
	@Buy				FLOAT	= NULL,
    @Sell				FLOAT	= NULL,
	@PmsId				INT
AS
BEGIN
    IF @TransactionTypeId = 3003
        RETURN

	DECLARE	@TempQty		SMALLINT = @Qty
	DECLARE @Temp2Qty		SMALLINT
	DECLARE @PortfolioId	INT
	DECLARE	@SellQty		SMALLINT
	DECLARE	@SellPrice		FLOAT	
	DECLARE	@SellBrokerage	FLOAT	
	DECLARE	@SellAmount		FLOAT	
	DECLARE	@BuyQty			SMALLINT
	DECLARE	@BuyPrice		FLOAT	
	DECLARE	@BuyBrokerage	FLOAT	
	DECLARE	@BuyAmount		FLOAT	

	WHILE	@TempQty > 0
	BEGIN
		IF @TransactionTypeId = 3001 
		BEGIN
			--buy

			SELECT		TOP 1	
						@PortfolioId	=   [Id], 
						@BuyQty			=	[BuyQty], 
						@BuyPrice		=	[BuyPrice],
						@BuyBrokerage	=	[BuyBrokerage],
						@BuyAmount		=	[BuyAmount],
						@SellQty		=	[SellQty], 
						@SellPrice		=	[SellPrice],
						@SellBrokerage	=	[SellBrokerage],
						@SellAmount		=	[SellAmount]
			FROM		[Portfolio] 
			WHERE		[AccountId]		= @AccountId 
				AND		[BrokerId]		= @BrokerId 
				AND		[ScriptId]		= @ScriptId 
				AND		[SellQty]		> [BuyQty]
			ORDER BY	Id

			IF  @PortfolioId IS  NULL 
			BEGIN
				-- Insert
				SET	@SellQty		= 0
				SET	@SellPrice		= 0
				SET	@SellBrokerage	= 0	
				SET	@SellAmount		= 0
				SET	@BuyQty			= @TempQty
				SET	@BuyPrice		= @Price	
				SET	@BuyBrokerage	= @Brokerage	 
				SET	@BuyAmount		= @Buy
				INSERT INTO [Portfolio] ( [AccountId], [BrokerId], [ScriptId], [BuyQty], [BuyPrice], [BuyBrokerage], [BuyAmount], [SellQty], [SellPrice], [SellBrokerage], [SellAmount],[PmsId])
							VALUES		( @AccountId,  @BrokerId,  @ScriptId,  @BuyQty,	@BuyPrice,  @BuyBrokerage,  @BuyAmount,  @SellQty,	@SellPrice,	 @SellBrokerage,  @SellAmount,	@PmsId )

				SET @PortfolioId = SCOPE_IDENTITY();

				INSERT INTO [TransactionProtfolio]		([Qty],		[TransactionId],	[ProtfolioId],	[DateTime],	[PmsId])
												VALUES	(@TempQty,	@TransactionId,		@PortfolioId,	@Date,		@PmsId);
				SET @TempQty = 0
						
			END
			ELSE
			BEGIN
				--Update incase of short sell
				IF	(@TempQty + @BuyQty) < @SellQty  
				BEGIN
					UPDATE	[Portfolio]  
					SET		[BuyQty]		= [BuyQty]			+ @TempQty,
							[BuyPrice]		= CONVERT(DECIMAL(10,2), ROUND((([BuyPrice] * [BuyQty]) + (@Price * @TempQty))	/ ([BuyQty] + @TempQty),2)),
							[BuyBrokerage]	= [BuyBrokerage]	+ @Brokerage,
							[BuyAmount]		= [BuyAmount]		+ CONVERT(DECIMAL(10,2), ROUND((@TempQty * @Price), 2)) + @Brokerage
					WHERE	[Id]			= @PortfolioId

					INSERT INTO [TransactionProtfolio] ([Qty],		[TransactionId],	[ProtfolioId],	[DateTime],	[PmsId])
												VALUES (@TempQty,	@TransactionId,		@PortfolioId,	@Date,		@PmsId);
					SET @TempQty = 0

				END
				ELSE
				BEGIN
					SET @Temp2Qty	= @SellQty - @BuyQty
					UPDATE	[Portfolio]  
					SET		[BuyQty]		= [BuyQty]			+ @Temp2Qty,
							[BuyPrice]		= CONVERT(DECIMAL(10,2), ROUND((([BuyPrice] * [BuyQty]) + (@Price * @Temp2Qty))	/ ([BuyQty] + @Temp2Qty),2)),
							[BuyBrokerage]	= [BuyBrokerage] + CONVERT(DECIMAL(10,2), ROUND((@Temp2Qty * @Brokerage) / @Qty, 2)),
							[BuyAmount]		= [BuyAmount] + CONVERT(DECIMAL(10,2), ROUND((@Temp2Qty * @Price), 2)) +  CONVERT(DECIMAL(10,2), ROUND((@Temp2Qty * @Brokerage) / @Qty, 2))
					WHERE	[Id]			= @PortfolioId

					INSERT INTO [TransactionProtfolio] ([Qty],		[TransactionId],	[ProtfolioId],	[DateTime],	[PmsId])
											VALUES	 (@Temp2Qty,	@TransactionId,		@PortfolioId,	@Date,		@PmsId);

					SET @TempQty = @TempQty - @Temp2Qty
				END
			END
		END
		ELSE IF @TransactionTypeId = 3002
		BEGIN
		--sell
			SELECT		TOP 1	
						@PortfolioId	=   [Id], 
						@BuyQty			=	[BuyQty], 
						@BuyPrice		=	[BuyPrice],
						@BuyBrokerage	=	[BuyBrokerage],
						@BuyAmount		=	[BuyAmount],
						@SellQty		=	[SellQty], 
						@SellPrice		=	[SellPrice],
						@SellBrokerage	=	[SellBrokerage],
						@SellAmount		=	[SellAmount]
			FROM		[Portfolio] 
			WHERE		[AccountId]		= @AccountId 
				AND		[BrokerId]		= @BrokerId 
				AND		[ScriptId]		= @ScriptId 
				AND		[BuyQty]		> [SellQty]
			ORDER BY	Id

			IF  @PortfolioId IS  NULL 
			BEGIN
				SET	@SellQty		= @TempQty
				SET	@SellPrice		= @Price	
				SET	@SellBrokerage	= @Brokerage	
				SET	@SellAmount		= @Sell
				SET	@BuyQty			= 0
				SET	@BuyPrice		= 0
				SET	@BuyBrokerage	= 0	
				SET	@BuyAmount		= 0
				INSERT INTO [Portfolio] ( [AccountId], [BrokerId], [ScriptId], [BuyQty], [BuyPrice], [BuyBrokerage], [BuyAmount], [SellQty], [SellPrice], [SellBrokerage], [SellAmount], [PmsId])
							VALUES		( @AccountId,  @BrokerId,  @ScriptId,  @BuyQty,	@BuyPrice,  @BuyBrokerage,  @BuyAmount,  @SellQty,	@SellPrice,	 @SellBrokerage,  @SellAmount, @PmsId )

				SET @PortfolioId = SCOPE_IDENTITY();

				INSERT INTO [TransactionProtfolio]		([Qty],		[TransactionId],	[ProtfolioId],	[DateTime],	[PmsId])
												VALUES	(@TempQty,	@TransactionId,		@PortfolioId,	@Date,		@PmsId);
				SET @TempQty = 0

			END
			ELSE
			BEGIN
				IF	(@TempQty + @SellQty) < @BuyQty
				BEGIN
					UPDATE	[Portfolio]  
					SET		[SellQty]		= [SellQty]			+ @TempQty,
							[SellPrice]		= CONVERT(DECIMAL(10,2), ROUND((([SellPrice] * [SellQty]) + (@Price * @TempQty))	/ ([SellQty] + @TempQty),2)),
							[SellBrokerage]	= [SellBrokerage]	+ @Brokerage,
							[SellAmount]	= [SellAmount]		+ CONVERT(DECIMAL(10,2), ROUND((@TempQty * @Price), 2)) - @Brokerage
					WHERE	[Id]			= @PortfolioId


					INSERT INTO [TransactionProtfolio]		([Qty],		[TransactionId],	[ProtfolioId],	[DateTime],	[PmsId])
												VALUES		(@TempQty,	@TransactionId,		@PortfolioId,	@Date,		@PmsId);
					SET @TempQty = 0

				END

				ELSE
				BEGIN
					SET @Temp2Qty	= @BuyQty - @SellQty
					UPDATE	[Portfolio]  
					SET		[SellQty]		= [SellQty]			+ @Temp2Qty,
							[SellPrice]		= CONVERT(DECIMAL(10,2), ROUND((([SellPrice] * [SellQty]) + (@Price * @Temp2Qty))	/ ([SellQty] + @Temp2Qty),2)),
							[SellBrokerage] = [SellBrokerage] +  CONVERT(DECIMAL(10,2), ROUND((@Temp2Qty * @Brokerage) / @Qty, 2)),
							[SellAmount]	= [SellAmount] + CONVERT(DECIMAL(10,2), ROUND((@Temp2Qty * @Price), 2)) - CONVERT(DECIMAL(10,2), ROUND((@Temp2Qty * @Brokerage) / @Qty, 2))
					WHERE	[Id]			= @PortfolioId


					INSERT INTO [TransactionProtfolio]		([Qty],		[TransactionId],	[ProtfolioId],	[DateTime],	[PmsId])
												VALUES	(@Temp2Qty,	@TransactionId,			@PortfolioId,	@Date,		@PmsId);

					SET @TempQty = @TempQty - @Temp2Qty
				END
			END
		END
	END
END
