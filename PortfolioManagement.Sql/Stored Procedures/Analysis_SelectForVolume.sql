CREATE PROCEDURE [dbo].[Analysis_SelectForVolume]
AS
BEGIN
	DECLARE @DateTime DATETIME
	SELECT	@DateTime = MAX(SP.[DateTime])
	FROM	DBO.[ScriptPrice] SP

	SELECT  
			Main.ScriptId,
			Main.ScriptName,
			Main.Volume,
			Main.WeekVolumeAverage,
			Main.WeekVolumePercentage,
			Main.MonthVolumeAverage,
			Main.MonthVolumePercentage,
			0 AS NewsCount
	FROM (	
			SELECT	R.ScriptId,
					R.ScriptName,
					R.Volume,
					R.WeekVolumeAverage,
					ROUND((R.Volume * 100 / R.WeekVolumeAverage), 2) AS WeekVolumePercentage,
					R.MonthVolumeAverage,
					ROUND((R.Volume * 100 / R.MonthVolumeAverage), 2) AS MonthVolumePercentage,
					0 AS NewsCount
			FROM	(
						SELECT			SP.ScriptId,
										S.NseCode AS [ScriptName],
										SP.Volume,
										(
											SELECT	AVG(SPI.Volume)
											FROM 	(	
														SELECT		TOP 5 SPII.Volume
														FROM		ScriptPrice SPII
														WHERE		SPII.ScriptId = SP.ScriptId
															AND		CONVERT(TIME, SPII.DateTime) = CONVERT(TIME, @DateTime)
														ORDER BY	SPII.Id DESC
													) SPI
										) AS WeekVolumeAverage,
										(
											SELECT	AVG(SPI.Volume)
											FROM 	(	
														SELECT		TOP 21 SPII.Volume
														FROM		ScriptPrice SPII
														WHERE		SPII.ScriptId = SP.ScriptId
															AND		CONVERT(TIME, SPII.DateTime) = CONVERT(TIME, @DateTime)
														ORDER BY	SPII.Id DESC
													) SPI
										) AS MonthVolumeAverage
						FROM			Script S
							INNER JOIN	ScriptPrice SP
									ON	S.Id				= SP.ScriptId
									AND	S.IsFetch			= 1
									AND	SP.[DateTime]		= @DateTime
			) R
					 
	) Main
	WHERE	WeekVolumePercentage  >= 125 
		OR	MonthVolumePercentage >= 125
	ORDER BY WeekVolumePercentage DESC;
END