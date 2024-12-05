CREATE PROCEDURE [dbo].[Index_Insert]
	@DateTime				datetime, 
	@SensexPreviousDay		float, 
	@SensexOpen				float, 
	@SensexClose			float, 
	@SensexHigh				float, 
	@SensexLow				float, 
	@NiftyPreviousDay		float, 
	@NiftyOpen				float, 
	@NiftyClose				float, 
	@NiftyHigh				float, 
	@NiftyLow				float,
	@Sensex					float,
	@Nifty					float
AS
/***********************************************************************************************
	 NAME     :  Index_Insert
	 PURPOSE  :  This SP insert record in table Index.
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        11/22/2020					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
		DECLARE @Id bigint
		DECLARE @Date date = CONVERT(DATE, @DateTime)

		IF NOT EXISTS(SELECT [Id] FROM Dbo.[Index] WHERE [Date] = @Date)
		BEGIN
			INSERT INTO Dbo.[Index] ([Date], [SensexPreviousDay], [SensexOpen], [SensexClose], [SensexHigh], [SensexLow], [NiftyPreviousDay], [NiftyOpen], [NiftyClose], [NiftyHigh], [NiftyLow], [Sensex], [Nifty]) 
			VALUES (@Datetime, @SensexPreviousDay, @SensexOpen, @SensexClose, @SensexHigh, @SensexLow, @NiftyPreviousDay, @NiftyOpen, @NiftyClose, @NiftyHigh, @NiftyLow, @Sensex, @Nifty)

		END
		ELSE
		BEGIN
			UPDATE	[Index] 
			SET		[SensexPreviousDay] = @SensexPreviousDay, 
					[SensexOpen]		= @SensexOpen, 
					[SensexClose]		= @SensexClose, 
					[SensexHigh]		= @SensexHigh, 
					[SensexLow]			= @SensexLow, 
					[NiftyPreviousDay]	= @NiftyPreviousDay, 
					[NiftyOpen]			= @NiftyOpen, 
					[NiftyClose]		= @NiftyClose, 
					[NiftyHigh]			= @NiftyHigh, 
					[NiftyLow]			= @NiftyLow,
					[Sensex]			= @Sensex,
					[Nifty]				= @Nifty
			WHERE	[Date]				= @Date;

		END
		
		INSERT INTO Dbo.[IndexRaw] ([DateTime], [SensexOpen], [SensexClose], [SensexHigh], [SensexLow], [NiftyOpen], [NiftyClose], [NiftyHigh], [NiftyLow], [Sensex], [Nifty]) 
		VALUES (@DateTime, @SensexOpen, @SensexClose, @SensexHigh, @SensexLow, @NiftyOpen, @NiftyClose, @NiftyHigh, @NiftyLow, @Sensex, @Nifty);


		SELECT SCOPE_IDENTITY();

END