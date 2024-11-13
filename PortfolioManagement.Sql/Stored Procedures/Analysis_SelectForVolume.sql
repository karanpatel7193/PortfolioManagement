CREATE PROCEDURE [dbo].[Analysis_SelectForVolume]
	@DateTime DATETIME
AS
BEGIN
	
	SELECT	R.ScriptId,
			R.ScriptName,
			R.Volume,
			R.WeekVolumnAverage,
			ROUND(R.WeekVolumnAverage * 100 / R.Volume, 2) AS WeekVolumnPercentage,
			R.MonthVolumnAverage,
			ROUND(R.MonthVolumnAverage * 100 / R.Volume, 2) AS MonthVolumnPercentage,
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
								) AS WeekVolumnAverage,
								(
									SELECT	AVG(SPI.Volume)
									FROM 	(	
												SELECT		TOP 21 SPII.Volume
												FROM		ScriptPrice SPII
												WHERE		SPII.ScriptId = SP.ScriptId
													AND		CONVERT(TIME, SPII.DateTime) = CONVERT(TIME, @DateTime)
												ORDER BY	SPII.Id DESC
											) SPI
								) AS MonthVolumnAverage
				FROM			Script S
					INNER JOIN	ScriptPrice SP
							ON	S.Id				= SP.ScriptId
							AND	S.IsFetch			= 1
							AND	SP.[DateTime]		= @DateTime
	) R

END