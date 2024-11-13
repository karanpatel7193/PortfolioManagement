CREATE TABLE [dbo].[DailySummary] (
    [Id]                BIGINT      IDENTITY (1, 1) NOT NULL,
    [Date]              DATE        NOT NULL,
    [SensexPreviousDay] FLOAT       NULL,
    [SensexOpen]        FLOAT       NULL,
    [SensexClose]       FLOAT       NULL,
    [SensexHigh]        FLOAT       NULL,
    [SensexLow]         FLOAT       NULL,
    [NiftyPreviousDay]  FLOAT       NULL,
    [NiftyOpen]         FLOAT       NULL,
    [NiftyClose]        FLOAT       NULL,
    [NiftyHigh]         FLOAT       NULL,
    [NiftyLow]          FLOAT       NULL,
    [FII]               FLOAT       NULL,
    [DII]               FLOAT       NULL,
    [NetBuy]            FLOAT       NULL, 
    [NetSell]           FLOAT       NULL, 
    [NetWorth]          FLOAT       NULL, 
    CONSTRAINT [PK_DailySummary] PRIMARY KEY CLUSTERED ([Id] ASC)
);

