/*
This SP select parent records from table Menu
Created By :: Rekansh Patel
Created On :: 05/27/2017
*/
CREATE PROCEDURE [dbo].[Menu_SelectParent]
AS
BEGIN
	SELECT A.[Id], A.[Name]
	FROM [Menu] A
	WHERE	A.ParentId IS NULL
	ORDER BY OrderBy
END

