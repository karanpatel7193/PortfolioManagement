CREATE   PROCEDURE dbo.[ScriptView_SelectForRange]
    @Id INT
AS
BEGIN

    SELECT 
					S.[ScriptId], 
					SP.[Name] AS ScriptName,
					SP.[NseCode],
					S.[Price],
					(S.[Price] - S.[PreviousDay]) AS PriceChange,
					((S.[Price] - S.[PreviousDay]) / S.[PreviousDay]) * 100 AS PricePercentage,
					S.[PreviousDay], 
					S.[High], 
					S.[Low], 
					S.[Volume], 
					S.[Value], 
					S.[High52Week], 
					S.[Low52Week],
					SP.[IndustryName]
    FROM			[ScriptLatestValue] S 

    INNER JOIN		[Script] SP 
			ON		SP.Id = S.ScriptId
    WHERE 
					S.[ScriptId] = @Id;

-- Select today's script price data
    SELECT 
					SP.[Price],
					SP.[Volume],
					CONVERT(DATETIME, SP.DateTime) AS [DateTime]
    FROM			dbo.ScriptPrice SP
    WHERE			SP.[ScriptId] = @Id
			AND		CONVERT(DATE, SP.DateTime) = CONVERT(DATE, GETDATE());
END;
