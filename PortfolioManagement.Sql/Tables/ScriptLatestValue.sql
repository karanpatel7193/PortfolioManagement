CREATE TABLE [dbo].[ScriptLatestValue] (
    [Id]          BIGINT     IDENTITY (1, 1) NOT NULL,
    [ScriptId]    SMALLINT   NOT NULL,
    [Price]       FLOAT (53) NOT NULL,
    [PreviousDay] FLOAT (53) NOT NULL,
    [Open]        FLOAT (53) NOT NULL,
    [Close]       FLOAT (53) NOT NULL,
    [High]        FLOAT (53) NOT NULL,
    [Low]         FLOAT (53) NOT NULL,
    [Volume]      INT        NOT NULL,
    [Value]       FLOAT (53) NOT NULL,
    [High52Week]  FLOAT (53) NOT NULL,
    [Low52Week]   FLOAT (53) NOT NULL,
    CONSTRAINT [PK_ScriptLatestValue] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ScriptLatestValue_Script] FOREIGN KEY ([ScriptId]) REFERENCES [dbo].[Script] ([Id])
);

