CREATE PROCEDURE [dbo].[SplitBonus_Insert]
    @ScriptID INT,
    @IsSplit BIT,
    @IsBonus BIT = NULL, 
    @OldFaceValue FLOAT = NULL,  
    @NewFaceValue FLOAT = NULL,  
    @FromRatio INT = NULL,
    @ToRatio INT = NULL,
    @AnnounceDate DATETIME,
    @RewardDate DATETIME,
    @IsApply BIT 
AS
BEGIN
    DECLARE @Id INT;

    IF NOT EXISTS (SELECT [Id] FROM [SplitBonus] WHERE [ScriptID] = @ScriptID)
    BEGIN
        INSERT INTO [SplitBonus] ([ScriptID], [IsSplit], [IsBonus], [OldFaceValue], [NewFaceValue], [FromRatio], [ToRatio], [AnnounceDate], [RewardDate], [IsApply])
        VALUES (@ScriptID, @IsSplit, @IsBonus, @OldFaceValue, @NewFaceValue, @FromRatio, @ToRatio, @AnnounceDate, @RewardDate, @IsApply);

        SET @Id = SCOPE_IDENTITY();
    END
    ELSE
        SET @Id = 0;
    
        SELECT @Id;
END
GO
