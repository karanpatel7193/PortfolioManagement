CREATE TABLE [dbo].[ScriptPrice] (
    [Id]          BIGINT     IDENTITY (1, 1) NOT NULL,
    [ScriptId]    SMALLINT   NOT NULL,
    [DateTime]    DATETIME   NOT NULL,
    [Price]       FLOAT (53) NOT NULL,
    [Volume]      BIGINT     NOT NULL,
    [PreviousDay] FLOAT (53) NULL,
    [High]        FLOAT (53) NULL,
    [Low]         FLOAT (53) NULL,
    [Open]        FLOAT (53) NULL,
    [Close]       FLOAT (53) NULL,
    CONSTRAINT [PK_ScriptPrice] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ScriptPrice_Script] FOREIGN KEY ([ScriptId]) REFERENCES [dbo].[Script] ([Id])
);



