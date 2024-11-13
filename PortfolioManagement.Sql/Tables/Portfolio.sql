CREATE TABLE [dbo].[Portfolio] (
    [Id]            BIGINT     IDENTITY (1, 1) NOT NULL,
    [AccountId]     TINYINT    NOT NULL,
    [BrokerId]      TINYINT    NOT NULL,
    [ScriptId]      SMALLINT   NOT NULL,
    [BuyQty]        SMALLINT   NOT NULL,
    [BuyPrice]      FLOAT (53) NOT NULL,
    [BuyBrokerage]  FLOAT (53) NULL,
    [BuyAmount]     FLOAT (53) NULL,
    [SellQty]       SMALLINT   NOT NULL,
    [SellPrice]     FLOAT (53) NOT NULL,
    [SellBrokerage] FLOAT (53) NULL,
    [SellAmount]    FLOAT (53) NULL,
    [PmsId]         INT        NULL,
    CONSTRAINT [PK_Protfolio] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Portfolio_Account] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_Portfolio_Broker] FOREIGN KEY ([BrokerId]) REFERENCES [dbo].[Broker] ([Id]),
    CONSTRAINT [FK_Portfolio_Script] FOREIGN KEY ([ScriptId]) REFERENCES [dbo].[Script] ([Id])
);




