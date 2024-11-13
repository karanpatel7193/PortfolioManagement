CREATE TABLE [dbo].[Index] (
    [Id]                BIGINT     IDENTITY (1, 1) NOT NULL,
    [Date]              DATE       NOT NULL,
    [SensexPreviousDay] FLOAT (53) NOT NULL,
    [SensexOpen]        FLOAT (53) NOT NULL,
    [SensexClose]       FLOAT (53) NOT NULL,
    [SensexHigh]        FLOAT (53) NOT NULL,
    [SensexLow]         FLOAT (53) NOT NULL,
    [NiftyPreviousDay]  FLOAT (53) NOT NULL,
    [NiftyOpen]         FLOAT (53) NOT NULL,
    [NiftyClose]        FLOAT (53) NOT NULL,
    [NiftyHigh]         FLOAT (53) NOT NULL,
    [NiftyLow]          FLOAT (53) NOT NULL,
    [FII]               FLOAT (53) NULL,
    [DII]               FLOAT (53) NULL,
    [Sensex] FLOAT NULL, 
    [Nifty] FLOAT NULL, 
    CONSTRAINT [PK_Index] PRIMARY KEY CLUSTERED ([Id] ASC)
);

