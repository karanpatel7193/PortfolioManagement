CREATE  PROCEDURE [dbo].[ScriptViewOverview_SelectForOverview]
    @Id INT
AS
BEGIN
    SELECT 
        S.[Id], 
        S.[ScriptId], 
		SP.[Name] AS ScriptName,
        SP.[NseCode],
        SP.[BseCode],
        S.[Price],
        S.[PreviousDay], 
        S.[Open], 
        S.[Close], 
        S.[High], 
        S.[Low], 
        S.[Volume], 
        S.[Value], 
        S.[High52Week], 
        S.[Low52Week],
        SP.[FaceValue]
    FROM 
				[ScriptLatestValue] S 
	INNER JOIN	[Script] SP
			ON	SP.Id = S.ScriptId
    WHERE 
        S.[ScriptId] = @Id
END
