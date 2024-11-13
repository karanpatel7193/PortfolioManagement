CREATE TABLE [dbo].[AccountBroker]
(
	[Id]					INT			IDENTITY (1, 1) NOT NULL,
	[BrokerId]				TINYINT     NOT NULL,
	[AccountId]				TINYINT     NOT NULL,
	CONSTRAINT [PK_AccountBroker] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountBroker_Account] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_AccountBroker_Broker] FOREIGN KEY ([BrokerId]) REFERENCES [dbo].[Broker] ([Id])
)
