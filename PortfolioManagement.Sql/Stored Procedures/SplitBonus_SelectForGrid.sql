CREATE PROCEDURE [dbo].[SplitBonus_SelectForGrid]
    @ScriptID INT = NULL,
    @IsSplit BIT = NULL,
    @IsBonus BIT = NULL
AS
BEGIN
    DECLARE @SqlQuery NVARCHAR(MAX)

    SET @SqlQuery = N'
    SELECT 
        SB.[Id], 
        S.[NseCode], -- Select NseCode from the Script table
        SB.[IsSplit], 
        SB.[IsBonus], 
        SB.[OldFaceValue], 
        SB.[NewFaceValue], 
        SB.[FromRatio], 
        SB.[ToRatio], 
        SB.[AnnounceDate], 
        SB.[RewardDate],
        SB.[IsApply]
    FROM [SplitBonus] SB
    INNER JOIN [Script] S ON SB.[ScriptID] = S.[Id]  -- Join with Script table to get NseCode
    WHERE 
        (@ScriptID IS NULL OR SB.[ScriptID] = @ScriptID) AND
        (@IsSplit IS NULL OR SB.[IsSplit] = @IsSplit) AND
        (@IsBonus IS NULL OR SB.[IsBonus] = @IsBonus);
    '

    EXEC sp_executesql @SqlQuery, 
                       N'@ScriptID INT, @IsSplit BIT, @IsBonus BIT', 
                       @ScriptID, @IsSplit, @IsBonus;

    SELECT COUNT(1) AS TotalRecords
    FROM [SplitBonus] SB
    INNER JOIN [Script] S ON SB.[ScriptID] = S.[Id]  -- Join with Script table to count correctly
    WHERE 
        (@ScriptID IS NULL OR SB.[ScriptID] = @ScriptID) AND
        (@IsSplit IS NULL OR SB.[IsSplit] = @IsSplit) AND
        (@IsBonus IS NULL OR SB.[IsBonus] = @IsBonus);
END
