CREATE PROCEDURE [dbo].[Account_SelectMaster]
    @Id TINYINT = NULL,
    @Name VARCHAR(20) = NULL
AS
BEGIN
    SELECT 
        A.[Id],
        A.[Name]
    FROM [dbo].[Account] A
    WHERE 
        A.[Id] = COALESCE(@Id, A.[Id])
        AND A.[Name] = COALESCE(@Name, A.[Name]);
END