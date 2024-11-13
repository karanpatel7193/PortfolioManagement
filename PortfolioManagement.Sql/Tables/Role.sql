CREATE TABLE [dbo].[Role] (
    [Id]       SMALLINT     IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsPublic] BIT          CONSTRAINT [DF_Role_IsPublic] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([Id] ASC)
);
