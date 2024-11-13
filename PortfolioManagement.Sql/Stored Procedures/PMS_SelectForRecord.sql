CREATE PROCEDURE [dbo].[PMS_SelectForRecord]
	@Id int
AS
BEGIN
	SELECT P.[Id], P.[Name], P.IsActive, P.[Type]
	FROM [PMS] P
	WHERE P.[Id] = @Id;
END
