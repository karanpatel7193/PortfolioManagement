CREATE PROCEDURE [dbo].[Master_SelectForGrid]
AS
BEGIN
	SELECT		M.[Id],	M.[Type]
	FROM		[Master] M
END