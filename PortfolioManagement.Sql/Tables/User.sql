CREATE TABLE [dbo].[User] (
    [Id]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [FirstName]          NVARCHAR (50)  NOT NULL,
    [MiddleName]         NVARCHAR (50)  NULL,
    [LastName]           NVARCHAR (50)  NOT NULL,
    [RoleId]             SMALLINT       NOT NULL,
    [Gender]             INT            NULL,
    [BirthDate]          DATETIME       NULL,
    [Username]           NVARCHAR (50)  NOT NULL,
    [Password]           VARCHAR (MAX)  NOT NULL,
    [PasswordSalt]       VARCHAR (MAX)  NOT NULL,
    [Email]              NVARCHAR (250) NOT NULL,
    [PhoneNumber]        VARCHAR (15)   NULL,
    [ImageSrc]           VARCHAR (MAX)  NULL,
    [IsActive]           BIT            CONSTRAINT [DF_User_IsActive] DEFAULT ((0)) NOT NULL,
    [LastUpdateDateTime] DATETIME       NULL,
    [PmsId]              INT            NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
);







    