CREATE PROCEDURE [dbo].[Account_SelectForEdit]
    @Id     INT,
    @PmsId  INT
AS

BEGIN
    EXEC [dbo].[Account_SelectForRecord] @Id 
    EXEC [dbo].[Account_SelectForAdd] @Id, @PmsId = @PmsId
END