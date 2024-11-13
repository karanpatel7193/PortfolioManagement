CREATE TABLE [dbo].[PortfolioDatewise]
(
	[Id]                INT         IDENTITY (1, 1) NOT NULL, 
    [PmsId]             INT         NOT NULL, 
    [BrokerId]          TINYINT     NOT NULL, 
    [AccountId]         TINYINT     NOT NULL, 
    [Date]              DATETIME    NOT NULL, 
    [InvestmentAmount]  FLOAT       NOT NULL, 
    [UnReleasedAmount]  FLOAT       NOT NULL,
    CONSTRAINT [PK_PortfolioDatewise] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PortfolioDatewise_PMS]       FOREIGN KEY ([PmsId])       REFERENCES [dbo].[PMS] ([Id]),
    CONSTRAINT [FK_PortfolioDatewise_Account]   FOREIGN KEY ([AccountId])   REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_PortfolioDatewise_Broker]    FOREIGN KEY ([BrokerId])    REFERENCES [dbo].[Broker] ([Id])
)
