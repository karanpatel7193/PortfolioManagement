CREATE TABLE [dbo].[StockTransaction] (
    [Id]                BIGINT     IDENTITY (1, 1) NOT NULL,
    [Date]              DATETIME   NOT NULL,
    [AccountId]         TINYINT    NOT NULL,
    [TransactionTypeId] INT        NOT NULL,
    [ScriptId]          SMALLINT   NOT NULL,
    [Qty]               SMALLINT   NOT NULL,
    [Price]             FLOAT (53) NOT NULL,
    [BrokerId]          TINYINT    NULL,
    [Brokerage]         FLOAT (53) NULL,
    [Buy]               FLOAT (53) NULL,
    [Sell]              FLOAT (53) NULL,
    [Dividend]          FLOAT (53) NULL,
    [PmsId]             INT        NULL,
    CONSTRAINT [PK_StockTransaction] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StockTransaction_Account] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_StockTransaction_Broker] FOREIGN KEY ([BrokerId]) REFERENCES [dbo].[Broker] ([Id]),
    CONSTRAINT [FK_StockTransaction_Master] FOREIGN KEY ([TransactionTypeId]) REFERENCES [dbo].[MasterValue] ([Value]),
    CONSTRAINT [FK_StockTransaction_Script] FOREIGN KEY ([ScriptId]) REFERENCES [dbo].[Script] ([Id])
);


GO
ALTER TABLE [dbo].[StockTransaction] NOCHECK CONSTRAINT [FK_StockTransaction_Account];


GO
ALTER TABLE [dbo].[StockTransaction] NOCHECK CONSTRAINT [FK_StockTransaction_Broker];


GO
ALTER TABLE [dbo].[StockTransaction] NOCHECK CONSTRAINT [FK_StockTransaction_Master];


GO
ALTER TABLE [dbo].[StockTransaction] NOCHECK CONSTRAINT [FK_StockTransaction_Script];




GO
ALTER TABLE [dbo].[StockTransaction] NOCHECK CONSTRAINT [FK_StockTransaction_Account];


GO
ALTER TABLE [dbo].[StockTransaction] NOCHECK CONSTRAINT [FK_StockTransaction_Broker];


GO
ALTER TABLE [dbo].[StockTransaction] NOCHECK CONSTRAINT [FK_StockTransaction_Master];


GO
ALTER TABLE [dbo].[StockTransaction] NOCHECK CONSTRAINT [FK_StockTransaction_Script];



