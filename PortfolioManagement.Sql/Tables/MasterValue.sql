CREATE TABLE [dbo].[MasterValue] (
    [MasterId]  SMALLINT       NOT NULL,
    [Value]     INT            NOT NULL,
    [ValueText] NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_MasterValue] PRIMARY KEY CLUSTERED ([Value] ASC),
    CONSTRAINT [FK_MasterValue_MasterValue] FOREIGN KEY ([MasterId]) REFERENCES [dbo].[Master] ([Id])
);

