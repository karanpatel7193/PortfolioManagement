CREATE PROCEDURE [dbo].[ScriptView_CorporateAction_SelectForBonus]
    @Id INT
AS
BEGIN
    SELECT 
        SB.[FromRatio], 
        SB.[ToRatio], 
        SB.[AnnounceDate], 
        SB.[RewardDate],
        SB.[IsBonus]

    FROM 
        [SplitBonus] SB
    WHERE 
        SB.[ScriptID] = @Id
        AND SB.IsBonus = 1
END
