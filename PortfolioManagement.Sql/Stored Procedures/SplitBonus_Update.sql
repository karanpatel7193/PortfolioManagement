CREATE PROCEDURE [dbo].[SplitBonus_Update]
    @Id INT,
    @ScriptID INT,
    @IsSplit BIT,
    @IsBonus BIT,
    @OldFaceValue FLOAT = NULL,  
    @NewFaceValue FLOAT = NULL,  
    @FromRatio INT = NULL,
    @ToRatio INT = NULL,
    @AnnounceDate DATETIME,
    @RewardDate DATETIME
AS
BEGIN
    -- Check for duplicate entries based on ScriptID
    IF NOT EXISTS(SELECT [Id] FROM [SplitBonus] 
                  WHERE [ScriptID] = @ScriptID AND [Id] != @Id)
    BEGIN
        -- Update the record
        UPDATE [SplitBonus]
        SET 
            [ScriptID] = @ScriptID,
            [IsSplit] = @IsSplit,
            [IsBonus] = @IsBonus,
            [OldFaceValue] = @OldFaceValue,
            [NewFaceValue] = @NewFaceValue,
            [FromRatio] = @FromRatio,
            [ToRatio] = @ToRatio,
            [AnnounceDate] = @AnnounceDate,
            [RewardDate] = @RewardDate
        WHERE [Id] = @Id;
    END
    ELSE
        SET @Id = 0;  -- Indicate that a duplicate entry exists

    SELECT @Id;  -- Return the Id of the updated record
END
GO
