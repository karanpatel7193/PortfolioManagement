CREATE TABLE [dbo].[Watchlist] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [PmsId] INT            NOT NULL,
    [Name]  NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Watchlist] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Watchlist_PMS] FOREIGN KEY ([PmsId]) REFERENCES [dbo].[PMS] ([Id])
);



