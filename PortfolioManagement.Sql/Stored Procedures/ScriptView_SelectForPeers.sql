CREATE PROCEDURE [dbo].[ScriptView_SelectForPeers]
	@Id		INT
AS
BEGIN

	DECLARE @IndustryName	VARCHAR(100)
	DECLARE @Group			VARCHAR(100)
	
	SELECT @IndustryName = IndustryName, @Group = [Group]
    FROM dbo.Script
    WHERE Id = @Id

    SELECT S.[Name], S.[Price]
    FROM dbo.Script S
    WHERE S.IndustryName = @IndustryName
      AND S.[Group] = @Group

END 
