CREATE TABLE [dbo].[Account] (
    [Id]    TINYINT      IDENTITY (1, 1) NOT NULL,
    [Name]  VARCHAR (20) NOT NULL,
    [PmsId] INT          NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Account_PMS1] FOREIGN KEY ([PmsId]) REFERENCES [dbo].[PMS] ([Id])
);



