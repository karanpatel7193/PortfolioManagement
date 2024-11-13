CREATE PROCEDURE [dbo].[SplitBonus_SelectForRecord]
    @Id INT
AS
BEGIN
    SELECT 
        SB.[Id], 
        SB.[ScriptID], 
        SB.[IsSplit], 
        SB.[IsBonus], 
        SB.[OldFaceValue], 
        SB.[NewFaceValue], 
        SB.[FromRatio], 
        SB.[ToRatio], 
        SB.[AnnounceDate], 
        SB.[RewardDate]
    FROM 
        [SplitBonus] SB
    WHERE 
        SB.[Id] = @Id;
END
