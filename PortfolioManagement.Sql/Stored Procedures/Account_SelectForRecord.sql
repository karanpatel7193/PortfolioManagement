CREATE PROCEDURE [dbo].[Account_SelectForRecord]
    @Id INT
AS
BEGIN
    SELECT A.[Id], A.[Name]
    FROM [dbo].[Account] A
    WHERE A.[Id] = @Id;
END