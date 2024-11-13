CREATE TABLE [dbo].[WatchlistScript] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [WatchListId] INT      NOT NULL,
    [ScriptId]    SMALLINT NOT NULL,
    [PmsId]       INT      NULL,
    CONSTRAINT [PK_WatchlistScript] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WatchlistScript_Script] FOREIGN KEY ([ScriptId]) REFERENCES [dbo].[Script] ([Id]),
    CONSTRAINT [FK_WatchlistScript_Watchlist] FOREIGN KEY ([WatchListId]) REFERENCES [dbo].[Watchlist] ([Id])
);



