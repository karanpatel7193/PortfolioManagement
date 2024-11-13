CREATE TABLE [dbo].[TransactionProtfolio] (
    [Id]            BIGINT   IDENTITY (1, 1) NOT NULL,
    [TransactionId] BIGINT   NOT NULL,
    [ProtfolioId]   BIGINT   NOT NULL,
    [Qty]           INT      NOT NULL,
    [DateTime]      DATETIME NULL,
    [PmsId]         INT      NULL,
    CONSTRAINT [PK_TransactionProtfolio] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TransactionProtfolio_StockTransaction] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[StockTransaction] ([Id])
);




