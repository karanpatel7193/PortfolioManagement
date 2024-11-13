CREATE PROCEDURE [dbo].[ScriptView_SelectForAboutCompany]
	@Id		INT
AS
BEGIN 
	SELECT	S.[Name],	S.[BseCode],	S.[NseCode],	S.[ISINCode],	S.[IndustryName] 
	FROM	dbo.Script S
	WHERE	S.Id = @Id
END