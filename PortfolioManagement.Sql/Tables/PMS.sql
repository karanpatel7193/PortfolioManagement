CREATE TABLE [dbo].[PMS] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (255) NOT NULL,
    [IsActive] BIT           CONSTRAINT [DF_PMS_IsActive] DEFAULT ((1)) NULL,
    [Type]     VARCHAR (100) NULL,
    CONSTRAINT [PK__PMS__3214EC0753BE0C05] PRIMARY KEY CLUSTERED ([Id] ASC)
);






