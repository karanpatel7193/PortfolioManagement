CREATE PROCEDURE dbo.[ScriptView_SelectForChart]
    @FromDate 		DATETIME,
    @ToDate			DATETIME,
	@ScriptId		INT
AS
BEGIN
			SELECT	S.Id,
					S.[Name]
			FROM	Dbo.[Script] S
			WHERE	S.ID = @ScriptId;

    DECLARE @DayRange INT;
    SET		@DayRange = DATEDIFF(DAY, @FromDate , @ToDate);

   -- IF @DayRange <= 7
   -- BEGIN
			--SELECT			SP.[Price],		SP.[Volume],	SP.[DateTime] AS [Date],	SP.[PreviousDay],	SP.[Open],	SP.[Close],		SP.[High],	SP.[Low]
			--FROM			dbo.ScriptPrice SP
			--WHERE			SP.DateTime >= @FromDate  
			--AND				SP.DateTime <= @ToDate	
			--AND				SP.[ScriptId] = @ScriptId;
   -- END
   -- ELSE
   -- BEGIN
			SELECT		SD.[Price],	SD.[Volume],	SD.[Date] AS [Date],	SD.[PreviousDay],	SD.[Open],	SD.[Close],		SD.[High],	SD.[Low]
			FROM		dbo.ScriptDaySummary SD
			WHERE		SD.Date >= @FromDate  
			AND			SD.Date <= @ToDate
			AND			SD.ScriptId = @ScriptId;
    --END
END;
