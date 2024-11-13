CREATE TABLE [dbo].[Menu] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (500) NULL,
    [ParentId]    INT           NULL,
    [PageTitle]   VARCHAR (50)  NULL,
    [Icon]        VARCHAR (250) NULL,
    [Routing]     VARCHAR (250) NULL,
    [OrderBy]     SMALLINT      NULL,
    [IsMenu]      BIT           CONSTRAINT [DF_Menu_IsWebPart] DEFAULT ((1)) NOT NULL,
    [IsClient]    BIT           CONSTRAINT [DF_Menu_IsClient] DEFAULT ((0)) NOT NULL,
    [IsPublic]    BIT           CONSTRAINT [DF_Menu_IsPublic] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Menu_Menu] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Menu] ([Id])
);

