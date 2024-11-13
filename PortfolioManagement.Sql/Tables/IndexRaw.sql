CREATE TABLE [dbo].[IndexRaw] (
    [Id]                BIGINT     IDENTITY (1, 1) NOT NULL,
    [DateTime]          DATETIME       NOT NULL,
    [SensexOpen]        FLOAT (53) NOT NULL,
    [SensexClose]       FLOAT (53) NOT NULL,
    [SensexHigh]        FLOAT (53) NOT NULL,
    [SensexLow]         FLOAT (53) NOT NULL,
    [NiftyOpen]         FLOAT (53) NOT NULL,
    [NiftyClose]        FLOAT (53) NOT NULL,
    [NiftyHigh]         FLOAT (53) NOT NULL,
    [NiftyLow]          FLOAT (53) NOT NULL,
    [Sensex] FLOAT NULL, 
    [Nifty] FLOAT NULL, 
    CONSTRAINT [PK_IndexRaw] PRIMARY KEY CLUSTERED ([Id] ASC)
);

