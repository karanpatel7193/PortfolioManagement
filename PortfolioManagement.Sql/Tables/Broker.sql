CREATE TABLE [dbo].[Broker] (
    [Id]            TINYINT      NOT NULL,
    [Name]          VARCHAR (20) NULL,
    [BrokerTypeId]  INT          NOT NULL,
    [BuyBrokerage]  FLOAT (53)   NULL,
    [SellBrokerage] FLOAT (53)   NULL,
    [PmsId]         INT          NULL,
    CONSTRAINT [PK_Broker] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Broker_Broker] FOREIGN KEY ([Id]) REFERENCES [dbo].[Broker] ([Id]),
    CONSTRAINT [FK_Broker_Master] FOREIGN KEY ([PmsId]) REFERENCES [dbo].[PMS] ([Id]),
    CONSTRAINT [FK_Broker_PMS] FOREIGN KEY ([PmsId]) REFERENCES [dbo].[PMS] ([Id])
);







