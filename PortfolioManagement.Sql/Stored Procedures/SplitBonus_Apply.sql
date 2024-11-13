CREATE PROCEDURE [dbo].[SplitBonus_Apply]
    @Id INT,
    @IsApply BIT
AS
BEGIN
    DECLARE 
    @ScriptID INT,
    @IsSplit BIT,
    @IsBonus BIT,
    @OldFaceValue FLOAT = NULL,  
    @NewFaceValue FLOAT = NULL,  
    @FromRatio INT = NULL,
    @ToRatio INT = NULL

    SELECT      @ScriptID     = SB.ScriptID,
                @IsSplit      = SB.IsSplit,
                @IsBonus      = SB.IsBonus,
                @OldFaceValue = SB.OldFaceValue,
                @NewFaceValue = SB.NewFaceValue,
                @FromRatio    = SB.FromRatio,
                @ToRatio      = SB.ToRatio
    FROM        SplitBonus SB
    WHERE       SB.Id = @Id
        
    BEGIN TRANSACTION BulkSplitBonusUpdate 
	BEGIN TRY
        -- Update IsApply only if the procedure is to be applied
        IF @IsApply = 1
        BEGIN
            UPDATE SplitBonus
            SET IsApply = 1
            WHERE Id = @Id;
        END

        IF @IsSplit = 1
        BEGIN
            UPDATE	dbo.[Script]
		    SET		[FaceValue]             = @NewFaceValue,
                    [Price]					= [Price] * @NewFaceValue / @OldFaceValue
		    WHERE	 Id						= @ScriptID

		    UPDATE	dbo.[StockTransaction]
		    SET		[ScriptId]				= @ScriptId		,	
				    [Qty]					= [Qty] * @OldFaceValue / @NewFaceValue,	
				    [Price]					= [Price] * @NewFaceValue / @OldFaceValue	
		    WHERE	ScriptId				= @ScriptID

            UPDATE	dbo.[ScriptPrice]
		    SET	    [Price]					= [Price] * @NewFaceValue / @OldFaceValue,
                    [Volume]                = [Volume] * @OldFaceValue / @NewFaceValue
		    WHERE	ScriptId				= @ScriptID

            UPDATE	        T
		    SET	            [Qty]           = T.[Qty] * @OldFaceValue / @NewFaceValue
            FROM            dbo.[TransactionProtfolio] T
                INNER JOIN  dbo.[StockTransaction] ST
                    ON      T.TransactionId = ST.Id
		    WHERE	ST.ScriptId		        = @ScriptID

            UPDATE	dbo.[Portfolio]
		    SET	    [BuyQty]				= [BuyQty] * @NewFaceValue / @OldFaceValue,
                    [BuyPrice]              = [BuyPrice] * @OldFaceValue / @NewFaceValue
		    WHERE	ScriptId				= @ScriptID

            UPDATE	dbo.[ScriptDaySummary]
		    SET	    [Price]					= [Price] * @NewFaceValue / @OldFaceValue,
                    [Volume]				= [Volume] * @NewFaceValue / @OldFaceValue,
                    [PreviousDay]           = [PreviousDay] * @OldFaceValue / @NewFaceValue,
                    [Open]                  = [Open] * @OldFaceValue / @NewFaceValue,
                    [Close]                 = [Close] * @OldFaceValue / @NewFaceValue,
                    [High]                  = [High] * @OldFaceValue / @NewFaceValue,
                    [Low]                   = [Low] * @OldFaceValue / @NewFaceValue,
                    [High52Week]            = [High52Week] * @OldFaceValue / @NewFaceValue,
                    [Low52Week]             = [Low52Week] * @OldFaceValue / @NewFaceValue
		    WHERE	ScriptId				= @ScriptID

            UPDATE	dbo.[ScriptLatestValue]
		    SET	    [Price]					= [Price] * @NewFaceValue / @OldFaceValue,
                    [Volume]				= [Volume] * @NewFaceValue / @OldFaceValue,
                    [PreviousDay]           = [PreviousDay] * @OldFaceValue / @NewFaceValue,
                    [Open]                  = [Open] * @OldFaceValue / @NewFaceValue,
                    [Close]                 = [Close] * @OldFaceValue / @NewFaceValue,
                    [High]                  = [High] * @OldFaceValue / @NewFaceValue,
                    [Low]                   = [Low] * @OldFaceValue / @NewFaceValue,
                    [High52Week]            = [High52Week] * @OldFaceValue / @NewFaceValue,
                    [Low52Week]             = [Low52Week] * @OldFaceValue / @NewFaceValue
		    WHERE	ScriptId				= @ScriptID
        END

        IF @IsBonus = 1
        BEGIN
            UPDATE	dbo.[Script]
		    SET		[Price]					= [Price] * @ToRatio / @FromRatio
		    WHERE	Id						= @ScriptID

		    UPDATE	dbo.[StockTransaction]
		    SET		[Qty]					= [Qty] * @FromRatio / @ToRatio,	
				    [Price]					= [Price] * @ToRatio / @FromRatio	
		    WHERE	ScriptId				= @ScriptID

            UPDATE	dbo.[ScriptPrice]
		    SET	    [Price]					= [Price] * @ToRatio / @FromRatio,
                    [Volume]                = [Volume] * @FromRatio / @ToRatio
		    WHERE	ScriptId				= @ScriptID

            UPDATE	        T
		    SET	            [Qty]           = T.[Qty] * @FromRatio / @ToRatio
            FROM            dbo.[TransactionProtfolio] T
                INNER JOIN  dbo.[StockTransaction] ST
                    ON      T.TransactionId = ST.Id
		    WHERE	ST.ScriptId		        = @ScriptID

            UPDATE	dbo.[Portfolio]
		    SET	    [BuyQty]				= [BuyQty] * @ToRatio / @FromRatio,
                    [BuyPrice]              = [BuyPrice] * @FromRatio / @ToRatio
		    WHERE	ScriptId				= @ScriptID

            UPDATE	dbo.[ScriptDaySummary]
		    SET	    [Price]					= [Price] * @ToRatio / @FromRatio,
                    [Volume]				= [Volume] * @ToRatio / @FromRatio,
                    [PreviousDay]           = [PreviousDay] * @FromRatio / @ToRatio,
                    [Open]                  = [Open] * @FromRatio / @ToRatio,
                    [Close]                 = [Close] * @FromRatio / @ToRatio,
                    [High]                  = [High] * @FromRatio / @ToRatio,
                    [Low]                   = [Low] * @FromRatio / @ToRatio,
                    [High52Week]            = [High52Week] * @FromRatio / @ToRatio,
                    [Low52Week]             = [Low52Week] * @FromRatio / @ToRatio
		    WHERE	ScriptId				= @ScriptID

            UPDATE	dbo.[ScriptLatestValue]
		    SET	    [Price]					= [Price] * @ToRatio / @FromRatio,
                    [Volume]				= [Volume] * @ToRatio / @FromRatio,
                    [PreviousDay]           = [PreviousDay] * @FromRatio / @ToRatio,
                    [Open]                  = [Open] * @FromRatio / @ToRatio,
                    [Close]                 = [Close] * @FromRatio / @ToRatio,
                    [High]                  = [High] * @FromRatio / @ToRatio,
                    [Low]                   = [Low] * @FromRatio / @ToRatio,
                    [High52Week]            = [High52Week] * @FromRatio / @ToRatio,
                    [Low52Week]             = [Low52Week] * @FromRatio / @ToRatio
		    WHERE	ScriptId				= @ScriptID
        END

        IF @@TRANCOUNT > 0
			COMMIT TRANSACTION BulkSplitBonusUpdate

    END TRY
	BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION BulkSplitBonusUpdate
	DECLARE @ErrMsg nvarchar(4000), @ErrSeverity int
	SELECT @ErrMsg = ERROR_MESSAGE(), @ErrSeverity = ERROR_SEVERITY();
	RAISERROR(@ErrMsg, @ErrSeverity, 1)
	END CATCH

END

