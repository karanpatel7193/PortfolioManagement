/*
This SP select records from table Menu
Created By :: Rekansh Patel
Created On :: 05/27/2017
*/
 CREATE PROCEDURE [dbo].[Menu_Select]
	@Id int = NULL,
	@Name varchar(50) = NULL,
	@IsMenu BIT = NULL
AS
BEGIN
	SELECT A.[Id], A.[Name], A.[Description], A.PageTitle, A.ParentId, A.Icon, A.Routing, A.IsMenu, A.IsClient, A.IsPublic, A.OrderBy,
		P.Name AS ParentIdName
	FROM [Menu] A
		LEFT JOIN [Menu] P ON A.ParentId = P.Id
	WHERE	A.[Id] = COALESCE(@Id, A.[Id])
		AND A.[Name] = COALESCE(@Name, A.[Name])
		AND A.IsMenu = COALESCE(@IsMenu, A.IsMenu)

END

