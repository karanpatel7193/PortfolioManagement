CREATE TABLE [dbo].[RoleMenuAccess] (
    [Id]        INT      IDENTITY (1, 1) NOT NULL,
    [RoleId]    SMALLINT NOT NULL,
    [MenuId]    INT      NOT NULL,
    [CanInsert] BIT      NOT NULL,
    [CanUpdate] BIT      NOT NULL,
    [CanDelete] BIT      NOT NULL,
    [CanView]   BIT      NOT NULL,
    CONSTRAINT [PK_RoleMenuAccess] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RoleMenuAccess_Menu] FOREIGN KEY ([MenuId]) REFERENCES [dbo].[Menu] ([Id]),
    CONSTRAINT [FK_RoleMenuAccess_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id])
);