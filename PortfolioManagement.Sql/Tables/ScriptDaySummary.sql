CREATE TABLE [dbo].[ScriptDaySummary] (
    [Id]          BIGINT     IDENTITY (1, 1) NOT NULL,
    [ScriptId]    SMALLINT   NOT NULL,
    [Date]        DATE       NOT NULL,
    [PreviousDay] FLOAT (53) NOT NULL,
    [Open]        FLOAT (53) NOT NULL,
    [Close]       FLOAT (53) NOT NULL,
    [High]        FLOAT (53) NOT NULL,
    [Low]         FLOAT (53) NOT NULL,
    [Volume]      BIGINT     NOT NULL,
    [Value]       FLOAT (53) NOT NULL,
    [High52Week]  FLOAT (53) NOT NULL,
    [Low52Week]   FLOAT (53) NOT NULL,
    [Price]       FLOAT (53) NOT NULL,
    CONSTRAINT [PK_ScriptDaySummary] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ScriptDaySummary_Script] FOREIGN KEY ([ScriptId]) REFERENCES [dbo].[Script] ([Id])
);



