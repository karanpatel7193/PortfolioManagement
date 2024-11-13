CREATE PROCEDURE [dbo].[ScriptView_CorporateAction_SelectForSplit]
    @Id INT
AS
BEGIN
    SELECT 
        SB.[OldFaceValue], 
        SB.[NewFaceValue], 
        SB.[AnnounceDate], 
        SB.[RewardDate],
        SB.[IsSplit]
    FROM 
        [SplitBonus] SB
    WHERE 
        SB.[ScriptID] = @Id
        AND SB.IsSplit = 1
END
