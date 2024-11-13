CREATE PROCEDURE [dbo].[Menu_SelectForRecord]
	@Id int
AS
/***********************************************************************************************
	 NAME     :  Menu_SelectForRecord
	 PURPOSE  :  This SP select perticular record from table Menu for open page in edit / view mode.
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        03/10/2018					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	SELECT M.[Id], M.[Name], M.[Description], M.[ParentId], M.[PageTitle], M.[Icon], M.[Routing], M.[OrderBy], M.[IsMenu], M.IsClient, M.IsPublic
	FROM [Menu] M
	WHERE M.[Id] = @Id;
END

