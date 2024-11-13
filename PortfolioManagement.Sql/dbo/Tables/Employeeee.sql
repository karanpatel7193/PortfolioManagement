CREATE TABLE [dbo].[Employeeee] (
    [EmployeeId] INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]  NVARCHAR (50) NULL,
    [LastName]   NVARCHAR (50) NULL,
    [Department] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([EmployeeId] ASC)
);

