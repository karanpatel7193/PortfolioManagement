CREATE TABLE [dbo].[SplitBonus] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [ScriptID]     INT      NOT NULL,
    [IsSplit]      BIT      NULL,
    [IsBonus]      BIT      NULL,
    [OldFaceValue] INT      NULL,
    [NewFaceValue] INT      NULL,
    [FromRatio]    INT      NULL,
    [ToRatio]      INT      NULL,
    [AnnounceDate] DATETIME NULL,
    [RewardDate]   DATETIME NULL,
    [IsApply]      BIT      DEFAULT ((0)) NULL,
    CONSTRAINT [PK__SplitBon__3214EC07C81010C4] PRIMARY KEY CLUSTERED ([Id] ASC)
);

