CREATE TABLE [dbo].[Script] (
    [Id]              SMALLINT      IDENTITY (1, 1) NOT NULL,
    [Name]            VARCHAR (300) NOT NULL,
    [BseCode]         NUMERIC (6)   NOT NULL,
    [NseCode]         VARCHAR (30)  NOT NULL,
    [ISINCode]        VARCHAR (30)  NOT NULL,
    [MoneyControlURL] VARCHAR (MAX) NOT NULL,
    [FetchURL]        VARCHAR (MAX) NOT NULL,
    [IsFetch]         BIT           NOT NULL,
    [IsActive]        BIT           NOT NULL,
    [Price]           FLOAT (53)    NULL,
    [IndustryName]    VARCHAR (MAX) NOT NULL,
    [Group]           VARCHAR (10)  NOT NULL,
    [FaceValue]       INT           NOT NULL,
    [IciciCode]       NCHAR (10)    NULL,
    [IsNifty50]       BIT           NULL,
    [PreviousDay]     FLOAT (53)    NULL,
    CONSTRAINT [PK_Script] PRIMARY KEY CLUSTERED ([Id] ASC)
);





